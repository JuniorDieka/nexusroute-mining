using Microsoft.EntityFrameworkCore;
using NexusRoute.Application.Interfaces;
using NexusRoute.Domain.Entities;
using NexusRoute.Infrastructure.Data;

namespace NexusRoute.Infrastructure.Repositories;

public class OreMovementRepository : IOreMovementRepository
{
    private readonly NexusRouteDbContext _context;

    public OreMovementRepository(NexusRouteDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<OreMovementLog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.OreMovementLogs
            .Include(o => o.Asset)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<OreMovementLog>> GetByAssetIdAsync(Guid assetId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var query = _context.OreMovementLogs
            .Where(o => o.AssetId == assetId);

        if (startDate.HasValue)
            query = query.Where(o => o.EventTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(o => o.EventTime <= endDate.Value);

        return await query
            .OrderByDescending(o => o.EventTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OreMovementLog>> GetByCycleIdAsync(Guid cycleId, CancellationToken cancellationToken = default)
    {
        return await _context.OreMovementLogs
            .Where(o => o.CycleId == cycleId)
            .OrderBy(o => o.EventTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OreMovementLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.OreMovementLogs
            .Where(o => o.EventTime >= startDate && o.EventTime <= endDate)
            .OrderByDescending(o => o.EventTime)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(OreMovementLog log, CancellationToken cancellationToken = default)
    {
        await _context.OreMovementLogs.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<OreMovementLog> logs, CancellationToken cancellationToken = default)
    {
        await _context.OreMovementLogs.AddRangeAsync(logs, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
