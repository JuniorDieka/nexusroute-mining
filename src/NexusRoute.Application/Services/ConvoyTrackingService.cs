using Microsoft.Extensions.Logging;
using NexusRoute.Application.DTOs;
using NexusRoute.Application.Interfaces;
using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Enums;
using NexusRoute.Domain.Services;
using NexusRoute.Domain.ValueObjects;

namespace NexusRoute.Application.Services;

public class ConvoyTrackingService
{
    private readonly IConvoyRepository _convoyRepository;
    private readonly IAssetRepository _assetRepository;
    private readonly IAlertRepository _alertRepository;
    private readonly IGeofenceValidator _geofenceValidator;
    private readonly INotificationHub _notificationHub;
    private readonly ILogger<ConvoyTrackingService> _logger;

    public ConvoyTrackingService(
        IConvoyRepository convoyRepository,
        IAssetRepository assetRepository,
        IAlertRepository alertRepository,
        IGeofenceValidator geofenceValidator,
        INotificationHub notificationHub,
        ILogger<ConvoyTrackingService> logger)
    {
        _convoyRepository = convoyRepository ?? throw new ArgumentNullException(nameof(convoyRepository));
        _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
        _alertRepository = alertRepository ?? throw new ArgumentNullException(nameof(alertRepository));
        _geofenceValidator = geofenceValidator ?? throw new ArgumentNullException(nameof(geofenceValidator));
        _notificationHub = notificationHub ?? throw new ArgumentNullException(nameof(notificationHub));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<ConvoyDto>> GetActiveConvoysAsync(CancellationToken cancellationToken = default)
    {
        var convoys = await _convoyRepository.GetActiveConvoysAsync(cancellationToken);
        
        var dtos = new List<ConvoyDto>();
        foreach (var convoy in convoys)
        {
            var asset = await _assetRepository.GetByIdAsync(convoy.LeadAssetId, cancellationToken);
            
            dtos.Add(new ConvoyDto
            {
                Id = convoy.Id,
                ConvoyCode = convoy.ConvoyCode,
                RouteId = convoy.RouteId,
                RouteName = convoy.Route?.Name ?? string.Empty,
                LeadAssetId = convoy.LeadAssetId,
                LeadAssetCode = convoy.LeadAsset?.AssetCode ?? string.Empty,
                CargoType = convoy.CargoType,
                CargoValueUsd = convoy.CargoValueUsd,
                ScheduledDepartureTime = convoy.ScheduledDepartureTime,
                ActualDepartureTime = convoy.ActualDepartureTime,
                ActualArrivalTime = convoy.ActualArrivalTime,
                Status = convoy.Status,
                IsHighPriority = convoy.IsHighPriority,
                CurrentLatitude = asset?.CurrentLatitude,
                CurrentLongitude = asset?.CurrentLongitude
            });
        }

        return dtos;
    }

    public async Task MonitorConvoyPositionAsync(Guid convoyId, CancellationToken cancellationToken = default)
    {
        var convoy = await _convoyRepository.GetByIdAsync(convoyId, cancellationToken);
        if (convoy == null || !convoy.IsInTransit)
        {
            return;
        }

        var asset = await _assetRepository.GetByIdAsync(convoy.LeadAssetId, cancellationToken);
        if (asset?.CurrentLatitude == null || asset.CurrentLongitude == null)
        {
            return;
        }

        var currentPosition = new GpsPosition(asset.CurrentLatitude.Value, asset.CurrentLongitude.Value);

        if (_geofenceValidator.HasDeviatedFromRoute(currentPosition, convoy.Route, convoy.Route.GeofenceRadiusKm))
        {
            var distance = _geofenceValidator.DistanceFromRoute(currentPosition, convoy.Route);
            var message = $"Convoy has deviated {distance:F2} km from assigned route";
            
            var alert = Alert.CreateConvoyAlert(
                convoy,
                AlertType.RouteDeviation,
                AlertSeverity.Critical,
                message,
                $"Current position: {currentPosition.Latitude:F6}, {currentPosition.Longitude:F6}"
            );

            await _alertRepository.AddAsync(alert, cancellationToken);
            convoy.MarkAsDeviated("Route deviation detected");
            await _convoyRepository.UpdateAsync(convoy, cancellationToken);

            var alertDto = new AlertDto
            {
                Id = alert.Id,
                Type = alert.Type,
                Severity = alert.Severity,
                ConvoyId = alert.ConvoyId,
                ConvoyCode = convoy.ConvoyCode,
                Title = alert.Title,
                Message = alert.Message,
                Details = alert.Details,
                CreatedAt = alert.CreatedAt,
                IsActive = alert.IsActive,
                IsAcknowledged = alert.IsAcknowledged
            };

            await _notificationHub.SendAlertAsync(alertDto, cancellationToken);
            
            _logger.LogCritical("Convoy {ConvoyCode} route deviation detected: {Distance} km", convoy.ConvoyCode, distance);
        }

        await CheckCheckpointComplianceAsync(convoy, currentPosition, cancellationToken);
    }

    private async Task CheckCheckpointComplianceAsync(Convoy convoy, GpsPosition currentPosition, CancellationToken cancellationToken)
    {
        if (!convoy.ActualDepartureTime.HasValue)
        {
            return;
        }

        foreach (var checkpoint in convoy.Route.Checkpoints.Where(c => c.IsActive && c.IsMandatory).OrderBy(c => c.SequenceNumber))
        {
            var checkpointLog = convoy.CheckpointLogs.FirstOrDefault(cl => cl.CheckpointId == checkpoint.Id);
            
            if (checkpointLog?.IsCompleted == true)
            {
                continue;
            }

            if (checkpoint.IsOverdue(convoy.ActualDepartureTime.Value, DateTime.UtcNow))
            {
                var message = $"Checkpoint '{checkpoint.Name}' missed or overdue";
                
                var alert = Alert.CreateConvoyAlert(
                    convoy,
                    AlertType.CheckpointMissed,
                    AlertSeverity.Critical,
                    message,
                    $"Expected at {convoy.ActualDepartureTime.Value.Add(checkpoint.ExpectedTimeFromStart):yyyy-MM-dd HH:mm:ss} UTC"
                );

                await _alertRepository.AddAsync(alert, cancellationToken);

                var alertDto = new AlertDto
                {
                    Id = alert.Id,
                    Type = alert.Type,
                    Severity = alert.Severity,
                    ConvoyId = alert.ConvoyId,
                    ConvoyCode = convoy.ConvoyCode,
                    Title = alert.Title,
                    Message = alert.Message,
                    Details = alert.Details,
                    CreatedAt = alert.CreatedAt,
                    IsActive = alert.IsActive,
                    IsAcknowledged = alert.IsAcknowledged
                };

                await _notificationHub.SendAlertAsync(alertDto, cancellationToken);
                
                _logger.LogCritical("Convoy {ConvoyCode} missed checkpoint {CheckpointName}", convoy.ConvoyCode, checkpoint.Name);
            }
        }
    }
}
