using FluentValidation;
using Microsoft.Extensions.Logging;
using NexusRoute.Application.DTOs;
using NexusRoute.Application.Interfaces;
using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Services;
using NexusRoute.Domain.ValueObjects;

namespace NexusRoute.Application.Services;

public class TelemetryService
{
    private readonly ITelemetryRepository _telemetryRepository;
    private readonly IAssetRepository _assetRepository;
    private readonly IAlertRepository _alertRepository;
    private readonly IHealthMonitor _healthMonitor;
    private readonly INotificationHub _notificationHub;
    private readonly IValidator<TelemetryDto> _validator;
    private readonly ILogger<TelemetryService> _logger;
    private readonly TelemetryThresholds _thresholds;

    public TelemetryService(
        ITelemetryRepository telemetryRepository,
        IAssetRepository assetRepository,
        IAlertRepository alertRepository,
        IHealthMonitor healthMonitor,
        INotificationHub notificationHub,
        IValidator<TelemetryDto> validator,
        ILogger<TelemetryService> logger)
    {
        _telemetryRepository = telemetryRepository ?? throw new ArgumentNullException(nameof(telemetryRepository));
        _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
        _alertRepository = alertRepository ?? throw new ArgumentNullException(nameof(alertRepository));
        _healthMonitor = healthMonitor ?? throw new ArgumentNullException(nameof(healthMonitor));
        _notificationHub = notificationHub ?? throw new ArgumentNullException(nameof(notificationHub));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _thresholds = TelemetryThresholds.CreateDefault();
    }

    public async Task<TelemetryResponseDto> IngestTelemetryAsync(TelemetryDto dto, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var asset = await _assetRepository.GetByIdAsync(dto.AssetId, cancellationToken);
        if (asset == null)
        {
            throw new InvalidOperationException($"Asset with ID {dto.AssetId} not found");
        }

        var telemetry = new Telemetry
        {
            Id = Guid.NewGuid(),
            AssetId = dto.AssetId,
            EngineTemperatureCelsius = dto.EngineTemperatureCelsius,
            PayloadWeightTonnes = dto.PayloadWeightTonnes,
            TirePressurePsi = dto.TirePressurePsi,
            FuelLevelPercentage = dto.FuelLevelPercentage,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Altitude = dto.Altitude,
            Timestamp = dto.Timestamp ?? DateTime.UtcNow,
            ReceivedAt = DateTime.UtcNow
        };

        await _telemetryRepository.AddAsync(telemetry, cancellationToken);

        asset.UpdatePosition(telemetry.GetPosition());
        await _assetRepository.UpdateAsync(asset, cancellationToken);

        var (severity, message) = _healthMonitor.EvaluateTelemetry(telemetry, _thresholds);
        if (severity.HasValue && message != null)
        {
            var alertType = _healthMonitor.DetermineAlertType(telemetry, _thresholds);
            if (alertType.HasValue)
            {
                var alert = Alert.CreateAssetHealthAlert(asset, alertType.Value, severity.Value, message);
                await _alertRepository.AddAsync(alert, cancellationToken);

                var alertDto = MapToAlertDto(alert);
                await _notificationHub.SendAlertAsync(alertDto, cancellationToken);
                
                _logger.LogWarning("Health alert created for asset {AssetCode}: {Message}", asset.AssetCode, message);
            }
        }

        var response = new TelemetryResponseDto
        {
            Id = telemetry.Id,
            AssetId = telemetry.AssetId,
            AssetCode = asset.AssetCode,
            EngineTemperatureCelsius = telemetry.EngineTemperatureCelsius,
            PayloadWeightTonnes = telemetry.PayloadWeightTonnes,
            TirePressurePsi = telemetry.TirePressurePsi,
            FuelLevelPercentage = telemetry.FuelLevelPercentage,
            Latitude = telemetry.Latitude,
            Longitude = telemetry.Longitude,
            Timestamp = telemetry.Timestamp
        };

        await _notificationHub.SendTelemetryUpdateAsync(response, cancellationToken);

        return response;
    }

    public async Task<IEnumerable<TelemetryResponseDto>> GetRecentTelemetryAsync(int minutes = 30, CancellationToken cancellationToken = default)
    {
        var telemetry = await _telemetryRepository.GetRecentAsync(minutes, cancellationToken);
        
        return telemetry.Select(t => new TelemetryResponseDto
        {
            Id = t.Id,
            AssetId = t.AssetId,
            AssetCode = t.Asset?.AssetCode ?? string.Empty,
            EngineTemperatureCelsius = t.EngineTemperatureCelsius,
            PayloadWeightTonnes = t.PayloadWeightTonnes,
            TirePressurePsi = t.TirePressurePsi,
            FuelLevelPercentage = t.FuelLevelPercentage,
            Latitude = t.Latitude,
            Longitude = t.Longitude,
            Timestamp = t.Timestamp
        });
    }

    private static AlertDto MapToAlertDto(Alert alert)
    {
        return new AlertDto
        {
            Id = alert.Id,
            Type = alert.Type,
            Severity = alert.Severity,
            AssetId = alert.AssetId,
            AssetCode = alert.Asset?.AssetCode,
            ConvoyId = alert.ConvoyId,
            ConvoyCode = alert.Convoy?.ConvoyCode,
            Title = alert.Title,
            Message = alert.Message,
            Details = alert.Details,
            CreatedAt = alert.CreatedAt,
            IsActive = alert.IsActive,
            IsAcknowledged = alert.IsAcknowledged
        };
    }
}
