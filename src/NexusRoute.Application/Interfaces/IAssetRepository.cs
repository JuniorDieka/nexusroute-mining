using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Enums;

namespace NexusRoute.Application.Interfaces;

public interface IAssetRepository
{
    Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Asset?> GetByAssetCodeAsync(string assetCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<Asset>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Asset>> GetByTypeAsync(AssetType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<Asset>> GetActiveAssetsAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Asset asset, CancellationToken cancellationToken = default);
    Task UpdateAsync(Asset asset, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
