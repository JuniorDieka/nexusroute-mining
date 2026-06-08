namespace NexusRoute.Domain.ValueObjects;

public record GpsPosition
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double? Altitude { get; init; }
    public DateTime Timestamp { get; init; }

    public GpsPosition(double latitude, double longitude, double? altitude = null, DateTime? timestamp = null)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90 degrees.", nameof(latitude));
        
        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180 degrees.", nameof(longitude));

        Latitude = latitude;
        Longitude = longitude;
        Altitude = altitude;
        Timestamp = timestamp ?? DateTime.UtcNow;
    }

    public double DistanceToKm(GpsPosition other)
    {
        const double earthRadiusKm = 6371.0;

        var lat1Rad = Latitude * Math.PI / 180.0;
        var lat2Rad = other.Latitude * Math.PI / 180.0;
        var deltaLatRad = (other.Latitude - Latitude) * Math.PI / 180.0;
        var deltaLonRad = (other.Longitude - Longitude) * Math.PI / 180.0;

        var a = Math.Sin(deltaLatRad / 2) * Math.Sin(deltaLatRad / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(deltaLonRad / 2) * Math.Sin(deltaLonRad / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return earthRadiusKm * c;
    }
}
