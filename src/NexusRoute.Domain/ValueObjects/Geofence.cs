namespace NexusRoute.Domain.ValueObjects;

public record Geofence
{
    public GpsPosition Center { get; init; }
    public double RadiusKm { get; init; }

    public Geofence(GpsPosition center, double radiusKm)
    {
        if (radiusKm <= 0)
            throw new ArgumentException("Radius must be greater than zero.", nameof(radiusKm));

        Center = center ?? throw new ArgumentNullException(nameof(center));
        RadiusKm = radiusKm;
    }

    public bool Contains(GpsPosition position)
    {
        if (position == null)
            throw new ArgumentNullException(nameof(position));

        var distance = Center.DistanceToKm(position);
        return distance <= RadiusKm;
    }

    public double DistanceFromBoundaryKm(GpsPosition position)
    {
        if (position == null)
            throw new ArgumentNullException(nameof(position));

        var distanceFromCenter = Center.DistanceToKm(position);
        return distanceFromCenter - RadiusKm;
    }
}
