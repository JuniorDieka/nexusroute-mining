using NexusRoute.Domain.Entities;
using NexusRoute.Domain.ValueObjects;

namespace NexusRoute.Domain.Services;

public interface IGeofenceValidator
{
    bool IsWithinGeofence(GpsPosition position, Geofence geofence);
    
    bool IsOnRoute(GpsPosition position, Route route);
    
    double DistanceFromRoute(GpsPosition position, Route route);
    
    bool HasDeviatedFromRoute(GpsPosition position, Route route, double maxDeviationKm);
}
