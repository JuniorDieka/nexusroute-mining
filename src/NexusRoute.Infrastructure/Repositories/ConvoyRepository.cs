using Microsoft.EntityFrameworkCore;
using NexusRoute.Application.Interfaces;
using NexusRoute.Domain.Entities;
using NexusRoute.Infrastructure.Data;

namespace NexusRoute.Infrastructure.Repositories;

public class ConvoyRepository : IConvoyRepository
{
    private readonly NexusRouteDbContext _context;

    public ConvoyRepository(NexusRouteDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Convoy?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Convoys
            .Include(c => c.Route)
            .Include(c => c.LeadAsset)
            .Include(c => c.CheckpointLogs)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Convoy?> GetByCodeAsync(string convoyCode, CancellationToken cancellationToken = default)
    {
        return await _context.Convoys
            .Include(c => c.Route)
            .Include(c => c.LeadAsset)
            .Include(c => c.CheckpointLogs)
            .FirstOrDefaultAsync(c => c.ConvoyCode == convoyCode, cancellationToken);
    }

    public async Task<IEnumerable<Convoy>> GetActiveConvoysAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Convoys
            .Include(c => c.Route)
            .Include(c => c.LeadAsset)
            .Where(c => c.IsActive)
            .OrderBy(c => c.ConvoyCode)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Convoy>> GetByRouteIdAsync(Guid routeId, CancellationToken cancellationToken = default)
    {
        return await _context.Convoys
            .Include(c => c.Route)
            .Include(c => c.LeadAsset)
            .Where(c => c.RouteId == routeId)
            .OrderByDescending(c => c.ScheduledDepartureTime)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Convoy convoy, CancellationToken cancellationToken = default)
    {
        await _context.Convoys.AddAsync(convoy, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Convoy convoy, CancellationToken cancellationToken = default)
    {
        _context.Convoys.Update(convoy);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
