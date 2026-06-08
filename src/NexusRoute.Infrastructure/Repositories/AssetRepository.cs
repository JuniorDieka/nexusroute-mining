using Microsoft.EntityFrameworkCore;
using NexusRoute.Application.Interfaces;
using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Enums;
using NexusRoute.Infrastructure.Data;

namespace NexusRoute.Infrastructure.Repositories;

public class AssetRepository : IAssetRepository
{
    private readonly NexusRouteDbContext _context;

    public AssetRepository(NexusRouteDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Assets
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Asset?> GetByAssetCodeAsync(string assetCode, CancellationToken cancellationToken = default)
    {
        return await _context.Assets
            .FirstOrDefaultAsync(a => a.AssetCode == assetCode, cancellationToken);
    }

    public async Task<IEnumerable<Asset>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Assets
            .OrderBy(a => a.AssetCode)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Asset>> GetByTypeAsync(AssetType type, CancellationToken cancellationToken = default)
    {
        return await _context.Assets
            .Where(a => a.Type == type)
            .OrderBy(a => a.AssetCode)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Asset>> GetActiveAssetsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Assets
            .Where(a => a.IsActive)
            .OrderBy(a => a.AssetCode)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Asset asset, CancellationToken cancellationToken = default)
    {
        await _context.Assets.AddAsync(asset, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Asset asset, CancellationToken cancellationToken = default)
    {
        _context.Assets.Update(asset);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var asset = await GetByIdAsync(id, cancellationToken);
        if (asset != null)
        {
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
