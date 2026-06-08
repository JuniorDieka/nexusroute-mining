using NexusRoute.Domain.Entities;
using NexusRoute.Domain.ValueObjects;

namespace NexusRoute.Domain.Services;

public class GeofenceValidator : IGeofenceValidator
{
    public bool IsWithinGeofence(GpsPosition position, Geofence geofence)
    {
        if (position == null || geofence == null)
            return false;

        return geofence.Contains(position);
    }

    public bool IsOnRoute(GpsPosition position, Route route)
    {
        if (position == null || route == null)
            return false;

        var routeGeofence = route.GetRouteGeofence();
        return routeGeofence.Contains(position);
    }

    public double DistanceFromRoute(GpsPosition position, Route route)
    {
        if (position == null || route == null)
            return double.MaxValue;

        var start = route.GetStartPosition();
        var end = route.GetEndPosition();

        var distanceToStart = position.DistanceToKm(start);
        var distanceToEnd = position.DistanceToKm(end);
        var routeLength = start.DistanceToKm(end);

        var minDistance = Math.Min(distanceToStart, distanceToEnd);

        if (routeLength > 0)
        {
            var t = Math.Max(0, Math.Min(1,
                ((position.Latitude - start.Latitude) * (end.Latitude - start.Latitude) +
                 (position.Longitude - start.Longitude) * (end.Longitude - start.Longitude)) /
                (Math.Pow(end.Latitude - start.Latitude, 2) + Math.Pow(end.Longitude - start.Longitude, 2))
            ));

            var projectionLat = start.Latitude + t * (end.Latitude - start.Latitude);
            var projectionLon = start.Longitude + t * (end.Longitude - start.Longitude);
            var projection = new GpsPosition(projectionLat, projectionLon);

            var perpendicularDistance = position.DistanceToKm(projection);
            minDistance = Math.Min(minDistance, perpendicularDistance);
        }

        return minDistance;
    }

    public bool HasDeviatedFromRoute(GpsPosition position, Route route, double maxDeviationKm)
    {
        if (position == null || route == null)
            return true;

        var distance = DistanceFromRoute(position, route);
        return distance > maxDeviationKm;
    }
}
