using NexusRoute.Domain.Entities;

namespace NexusRoute.Application.Interfaces;

public interface IConvoyRepository
{
    Task<Convoy?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Convoy?> GetByCodeAsync(string convoyCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<Convoy>> GetActiveConvoysAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Convoy>> GetByRouteIdAsync(Guid routeId, CancellationToken cancellationToken = default);
    Task AddAsync(Convoy convoy, CancellationToken cancellationToken = default);
    Task UpdateAsync(Convoy convoy, CancellationToken cancellationToken = default);
}
