using Microsoft.AspNetCore.SignalR;
using NexusRoute.Api.Hubs;
using NexusRoute.Application.DTOs;
using NexusRoute.Application.Interfaces;

namespace NexusRoute.Api.Services;

public class NotificationService : INotificationHub
{
    private readonly IHubContext<DispatchHub, IDispatchClient> _hubContext;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IHubContext<DispatchHub, IDispatchClient> hubContext,
        ILogger<NotificationService> logger)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendTelemetryUpdateAsync(TelemetryResponseDto telemetry, CancellationToken cancellationToken = default)
    {
        try
        {
            await _hubContext.Clients.All.ReceiveTelemetryUpdate(telemetry);
            _logger.LogDebug("Telemetry update sent for asset {AssetId}", telemetry.AssetId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send telemetry update");
        }
    }

    public async Task SendAssetStatusUpdateAsync(AssetStatusDto assetStatus, CancellationToken cancellationToken = default)
    {
        try
        {
            await _hubContext.Clients.All.ReceiveAssetStatusUpdate(assetStatus);
            _logger.LogDebug("Asset status update sent for asset {AssetId}", assetStatus.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send asset status update");
        }
    }

    public async Task SendAlertAsync(AlertDto alert, CancellationToken cancellationToken = default)
    {
        try
        {
            await _hubContext.Clients.All.ReceiveAlert(alert);
            _logger.LogInformation("Alert sent: {AlertType} - {Message}", alert.Type, alert.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send alert");
        }
    }

    public async Task SendConvoyUpdateAsync(ConvoyDto convoy, CancellationToken cancellationToken = default)
    {
        try
        {
            await _hubContext.Clients.All.ReceiveConvoyUpdate(convoy);
            _logger.LogDebug("Convoy update sent for convoy {ConvoyId}", convoy.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send convoy update");
        }
    }

    public async Task SendProductionUpdateAsync(ProductionSummaryDto production, CancellationToken cancellationToken = default)
    {
        try
        {
            await _hubContext.Clients.All.ReceiveProductionUpdate(production);
            _logger.LogDebug("Production update sent");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send production update");
        }
    }
}
