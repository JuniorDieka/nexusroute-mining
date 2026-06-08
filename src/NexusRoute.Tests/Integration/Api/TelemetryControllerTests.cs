using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NexusRoute.Application.DTOs;
using NexusRoute.Domain.Enums;
using NexusRoute.Infrastructure.Data;
using Xunit;

namespace NexusRoute.Tests.Integration.Api;

public class TelemetryControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public TelemetryControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<NexusRouteDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<NexusRouteDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetRecentTelemetry_ReturnsOkResult()
    {
        var response = await _client.GetAsync("/api/telemetry/recent?minutes=30");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var telemetry = await response.Content.ReadFromJsonAsync<List<TelemetryResponseDto>>();
        telemetry.Should().NotBeNull();
    }

    [Fact]
    public async Task IngestTelemetry_WithValidData_ReturnsCreated()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<NexusRouteDbContext>();
        
        var asset = new Domain.Entities.Asset
        {
            Id = Guid.NewGuid(),
            AssetCode = "TEST-001",
            Name = "Test Asset",
            Type = AssetType.HaulTruck,
            Status = AssetStatus.Idle,
            IsActive = true
        };
        
        context.Assets.Add(asset);
        await context.SaveChangesAsync();

        var telemetryDto = new TelemetryDto
        {
            AssetId = asset.Id,
            EngineTemperatureCelsius = 85.0,
            PayloadWeightTonnes = 150.0,
            TirePressurePsi = 100.0,
            FuelLevelPercentage = 75.0,
            Latitude = -2.1234,
            Longitude = 28.5678
        };

        var response = await _client.PostAsJsonAsync("/api/telemetry", telemetryDto);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task IngestTelemetry_WithInvalidData_ReturnsBadRequest()
    {
        var invalidDto = new TelemetryDto
        {
            AssetId = Guid.NewGuid(),
            EngineTemperatureCelsius = 500.0,
            PayloadWeightTonnes = -10.0,
            TirePressurePsi = 100.0,
            FuelLevelPercentage = 150.0,
            Latitude = -2.1234,
            Longitude = 28.5678
        };

        var response = await _client.PostAsJsonAsync("/api/telemetry", invalidDto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
