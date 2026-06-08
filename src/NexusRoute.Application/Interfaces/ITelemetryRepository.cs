using NexusRoute.Domain.Entities;

namespace NexusRoute.Application.Interfaces;

public interface ITelemetryRepository
{
    Task<Telemetry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Telemetry>> GetByAssetIdAsync(Guid assetId, int limit = 100, CancellationToken cancellationToken = default);
    Task<IEnumerable<Telemetry>> GetRecentAsync(int minutes = 30, CancellationToken cancellationToken = default);
    Task<Telemetry?> GetLatestForAssetAsync(Guid assetId, CancellationToken cancellationToken = default);
    Task AddAsync(Telemetry telemetry, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Telemetry> telemetry, CancellationToken cancellationToken = default);
}
