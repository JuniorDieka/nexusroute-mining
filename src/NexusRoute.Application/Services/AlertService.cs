using Microsoft.Extensions.Logging;
using NexusRoute.Application.DTOs;
using NexusRoute.Application.Interfaces;

namespace NexusRoute.Application.Services;

public class AlertService
{
    private readonly IAlertRepository _alertRepository;
    private readonly ILogger<AlertService> _logger;

    public AlertService(
        IAlertRepository alertRepository,
        ILogger<AlertService> logger)
    {
        _alertRepository = alertRepository ?? throw new ArgumentNullException(nameof(alertRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<AlertDto>> GetActiveAlertsAsync(CancellationToken cancellationToken = default)
    {
        var alerts = await _alertRepository.GetActiveAlertsAsync(cancellationToken);
        
        return alerts.Select(a => new AlertDto
        {
            Id = a.Id,
            Type = a.Type,
            Severity = a.Severity,
            AssetId = a.AssetId,
            AssetCode = a.Asset?.AssetCode,
            ConvoyId = a.ConvoyId,
            ConvoyCode = a.Convoy?.ConvoyCode,
            Title = a.Title,
            Message = a.Message,
            Details = a.Details,
            CreatedAt = a.CreatedAt,
            IsActive = a.IsActive,
            IsAcknowledged = a.IsAcknowledged
        });
    }

    public async Task AcknowledgeAlertAsync(Guid alertId, string acknowledgedBy, CancellationToken cancellationToken = default)
    {
        var alert = await _alertRepository.GetByIdAsync(alertId, cancellationToken);
        if (alert == null)
        {
            throw new InvalidOperationException($"Alert with ID {alertId} not found");
        }

        alert.Acknowledge(acknowledgedBy);
        await _alertRepository.UpdateAsync(alert, cancellationToken);

        _logger.LogInformation("Alert {AlertId} acknowledged by {User}", alertId, acknowledgedBy);
    }
}
