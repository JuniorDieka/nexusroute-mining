using FluentAssertions;
using NexusRoute.Domain.Entities;
using NexusRoute.Domain.Enums;
using NexusRoute.Domain.Services;
using Xunit;

namespace NexusRoute.Tests.Unit.Domain;

public class CycleTimeCalculatorTests
{
    private readonly CycleTimeCalculator _calculator;

    public CycleTimeCalculatorTests()
    {
        _calculator = new CycleTimeCalculator();
    }

    [Fact]
    public void CalculateCycleTime_WithValidLoadAndDumpEvents_ReturnsValidCycleTime()
    {
        var assetId = Guid.NewGuid();
        var events = new List<OreMovementLog>
        {
            new()
            {
                AssetId = assetId,
                EventType = "Load",
                EventTime = DateTime.UtcNow.AddMinutes(-20),
                MaterialType = MaterialType.Ore,
                TonnageEstimate = 180
            },
            new()
            {
                AssetId = assetId,
                EventType = "Dump",
                EventTime = DateTime.UtcNow,
                MaterialType = MaterialType.Ore,
                TonnageEstimate = 180
            }
        };

        var result = _calculator.CalculateCycleTime(events);

        result.Should().NotBeNull();
        result!.TotalTime.Should().BeGreaterThan(TimeSpan.Zero);
    }

    [Fact]
    public void CalculateCycleTime_WithInsufficientEvents_ReturnsNull()
    {
        var events = new List<OreMovementLog>
        {
            new()
            {
                EventType = "Load",
                EventTime = DateTime.UtcNow
            }
        };

        var result = _calculator.CalculateCycleTime(events);

        result.Should().BeNull();
    }

    [Fact]
    public void DetectBottleneck_WithExcessiveQueueTime_ReturnsTrue()
    {
        var cycleTime = new NexusRoute.Domain.ValueObjects.CycleTime(
            loadTime: TimeSpan.FromMinutes(2),
            haulTime: TimeSpan.FromMinutes(10),
            dumpTime: TimeSpan.FromMinutes(1.5),
            returnTime: TimeSpan.FromMinutes(8),
            queueTime: TimeSpan.FromMinutes(15)
        );

        var threshold = TimeSpan.FromMinutes(10);
        var result = _calculator.DetectBottleneck(cycleTime, threshold);

        result.Should().BeTrue();
    }
}
