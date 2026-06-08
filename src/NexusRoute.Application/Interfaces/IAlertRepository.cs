using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Enums;

namespace NexusRoute.Application.Interfaces;

public interface IAlertRepository
{
    Task<Alert?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Alert>> GetActiveAlertsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Alert>> GetByAssetIdAsync(Guid assetId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Alert>> GetByConvoyIdAsync(Guid convoyId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Alert>> GetBySeverityAsync(AlertSeverity severity, CancellationToken cancellationToken = default);
    Task AddAsync(Alert alert, CancellationToken cancellationToken = default);
    Task UpdateAsync(Alert alert, CancellationToken cancellationToken = default);
}
