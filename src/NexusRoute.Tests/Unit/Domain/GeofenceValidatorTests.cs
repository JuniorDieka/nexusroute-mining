using FluentAssertions;
using NexusRoute.Domain.Services;
using NexusRoute.Domain.ValueObjects;
using Xunit;

namespace NexusRoute.Tests.Unit.Domain;

public class GeofenceValidatorTests
{
    private readonly GeofenceValidator _validator;

    public GeofenceValidatorTests()
    {
        _validator = new GeofenceValidator();
    }

    [Fact]
    public void IsWithinGeofence_PositionInsideRadius_ReturnsTrue()
    {
        var center = new GpsPosition(-2.1234, 28.5678);
        var geofence = new Geofence(center, 1.0);
        var position = new GpsPosition(-2.1235, 28.5679);

        var result = _validator.IsWithinGeofence(position, geofence);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsWithinGeofence_PositionOutsideRadius_ReturnsFalse()
    {
        var center = new GpsPosition(-2.1234, 28.5678);
        var geofence = new Geofence(center, 0.5);
        var position = new GpsPosition(-2.2000, 28.6000);

        var result = _validator.IsWithinGeofence(position, geofence);

        result.Should().BeFalse();
    }

    [Fact]
    public void HasDeviatedFromRoute_PositionFarFromRoute_ReturnsTrue()
    {
        var route = new NexusRoute.Domain.Entities.Route
        {
            StartLatitude = -2.1234,
            StartLongitude = 28.5678,
            EndLatitude = -2.1278,
            EndLongitude = 28.5734,
            GeofenceRadiusKm = 1.0
        };

        var position = new GpsPosition(-2.2000, 28.6500);
        var maxDeviation = 2.0;

        var result = _validator.HasDeviatedFromRoute(position, route, maxDeviation);

        result.Should().BeTrue();
    }
}
