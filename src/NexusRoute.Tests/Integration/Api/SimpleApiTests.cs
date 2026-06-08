using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace NexusRoute.Tests.Integration.Api;

public class SimpleApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SimpleApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsHealthy()
    {
        var response = await _client.GetAsync("/health");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAssets_ReturnsOkResult()
    {
        var response = await _client.GetAsync("/api/assets");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetProductionSummary_ReturnsOkResult()
    {
        var response = await _client.GetAsync("/api/production/summary");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("totalTonnage");
    }

    [Fact]
    public async Task GetActiveAlerts_ReturnsOkResult()
    {
        var response = await _client.GetAsync("/api/alerts/active");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetActiveConvoys_ReturnsOkResult()
    {
        var response = await _client.GetAsync("/api/convoys/active");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
