using NexusRoute.Application.DTOs;

namespace NexusRoute.Application.Interfaces;

public interface INotificationHub
{
    Task SendTelemetryUpdateAsync(TelemetryResponseDto telemetry, CancellationToken cancellationToken = default);
    Task SendAssetStatusUpdateAsync(AssetStatusDto assetStatus, CancellationToken cancellationToken = default);
    Task SendAlertAsync(AlertDto alert, CancellationToken cancellationToken = default);
    Task SendConvoyUpdateAsync(ConvoyDto convoy, CancellationToken cancellationToken = default);
    Task SendProductionUpdateAsync(ProductionSummaryDto production, CancellationToken cancellationToken = default);
}
