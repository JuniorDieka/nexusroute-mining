using Microsoft.EntityFrameworkCore;
using NexusRoute.Application.Interfaces;
using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Enums;
using NexusRoute.Infrastructure.Data;

namespace NexusRoute.Infrastructure.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly NexusRouteDbContext _context;

    public AlertRepository(NexusRouteDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Alert?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Alerts
            .Include(a => a.Asset)
            .Include(a => a.Convoy)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Alert>> GetActiveAlertsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Alerts
            .Include(a => a.Asset)
            .Include(a => a.Convoy)
            .Where(a => a.IsActive)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Alert>> GetByAssetIdAsync(Guid assetId, CancellationToken cancellationToken = default)
    {
        return await _context.Alerts
            .Include(a => a.Asset)
            .Where(a => a.AssetId == assetId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Alert>> GetByConvoyIdAsync(Guid convoyId, CancellationToken cancellationToken = default)
    {
        return await _context.Alerts
            .Include(a => a.Convoy)
            .Where(a => a.ConvoyId == convoyId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Alert>> GetBySeverityAsync(AlertSeverity severity, CancellationToken cancellationToken = default)
    {
        return await _context.Alerts
            .Include(a => a.Asset)
            .Include(a => a.Convoy)
            .Where(a => a.Severity == severity && a.IsActive)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Alert alert, CancellationToken cancellationToken = default)
    {
        await _context.Alerts.AddAsync(alert, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Alert alert, CancellationToken cancellationToken = default)
    {
        _context.Alerts.Update(alert);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
