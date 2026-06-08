using NexusRoute.Domain.Entities;

namespace NexusRoute.Application.Interfaces;

public interface IOreMovementRepository
{
    Task<OreMovementLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OreMovementLog>> GetByAssetIdAsync(Guid assetId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<OreMovementLog>> GetByCycleIdAsync(Guid cycleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OreMovementLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task AddAsync(OreMovementLog log, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<OreMovementLog> logs, CancellationToken cancellationToken = default);
}
