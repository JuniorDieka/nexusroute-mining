using Microsoft.EntityFrameworkCore;
using NexusRoute.Application.Interfaces;
using NexusRoute.Domain.Entities;
using NexusRoute.Infrastructure.Data;

namespace NexusRoute.Infrastructure.Repositories;

public class TelemetryRepository : ITelemetryRepository
{
    private readonly NexusRouteDbContext _context;

    public TelemetryRepository(NexusRouteDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Telemetry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Telemetry
            .Include(t => t.Asset)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Telemetry>> GetByAssetIdAsync(Guid assetId, int limit = 100, CancellationToken cancellationToken = default)
    {
        return await _context.Telemetry
            .Where(t => t.AssetId == assetId)
            .OrderByDescending(t => t.Timestamp)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Telemetry>> GetRecentAsync(int minutes = 30, CancellationToken cancellationToken = default)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-minutes);
        return await _context.Telemetry
            .Include(t => t.Asset)
            .Where(t => t.Timestamp >= cutoff)
            .OrderByDescending(t => t.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<Telemetry?> GetLatestForAssetAsync(Guid assetId, CancellationToken cancellationToken = default)
    {
        return await _context.Telemetry
            .Where(t => t.AssetId == assetId)
            .OrderByDescending(t => t.Timestamp)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(Telemetry telemetry, CancellationToken cancellationToken = default)
    {
        await _context.Telemetry.AddAsync(telemetry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Telemetry> telemetry, CancellationToken cancellationToken = default)
    {
        await _context.Telemetry.AddRangeAsync(telemetry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
