using NexusRoute.Domain.Entities;
using NexusRoute.Domain.ValueObjects;

namespace NexusRoute.Domain.Services;

public class CycleTimeCalculator : ICycleTimeCalculator
{
    public CycleTime? CalculateCycleTime(IEnumerable<OreMovementLog> cycleEvents)
    {
        var events = cycleEvents.OrderBy(e => e.EventTime).ToList();
        
        if (events.Count < 2)
            return null;

        var loadEvent = events.FirstOrDefault(e => e.EventType.Equals("Load", StringComparison.OrdinalIgnoreCase));
        var dumpEvent = events.FirstOrDefault(e => e.EventType.Equals("Dump", StringComparison.OrdinalIgnoreCase));

        if (loadEvent == null || dumpEvent == null)
            return null;

        var loadTime = TimeSpan.FromMinutes(2);
        var haulTime = dumpEvent.EventTime - loadEvent.EventTime - loadTime;
        var dumpTime = TimeSpan.FromMinutes(1.5);
        var returnTime = TimeSpan.FromMinutes(haulTime.TotalMinutes * 0.8);
        var queueTime = TimeSpan.Zero;

        var queueEvent = events.FirstOrDefault(e => e.EventType.Equals("Queue", StringComparison.OrdinalIgnoreCase));
        if (queueEvent != null)
        {
            var nextEvent = events.FirstOrDefault(e => e.EventTime > queueEvent.EventTime);
            if (nextEvent != null)
            {
                queueTime = nextEvent.EventTime - queueEvent.EventTime;
            }
        }

        if (haulTime < TimeSpan.Zero)
            haulTime = TimeSpan.FromMinutes(15);

        return new CycleTime(loadTime, haulTime, dumpTime, returnTime, queueTime);
    }

    public IEnumerable<CycleTime> CalculateCycleTimes(IEnumerable<OreMovementLog> logs, Guid assetId)
    {
        var assetLogs = logs
            .Where(l => l.AssetId == assetId)
            .OrderBy(l => l.EventTime)
            .ToList();

        var cycles = new List<CycleTime>();
        var currentCycle = new List<OreMovementLog>();

        foreach (var log in assetLogs)
        {
            currentCycle.Add(log);

            if (log.EventType.Equals("Dump", StringComparison.OrdinalIgnoreCase))
            {
                var cycleTime = CalculateCycleTime(currentCycle);
                if (cycleTime != null)
                {
                    cycles.Add(cycleTime);
                }
                currentCycle.Clear();
            }
        }

        return cycles;
    }

    public TimeSpan CalculateAverageCycleTime(IEnumerable<CycleTime> cycles)
    {
        var cycleList = cycles.ToList();
        
        if (!cycleList.Any())
            return TimeSpan.Zero;

        var averageTicks = cycleList.Average(c => c.TotalTime.Ticks);
        return TimeSpan.FromTicks((long)averageTicks);
    }

    public bool DetectBottleneck(CycleTime cycleTime, TimeSpan queueThreshold)
    {
        return cycleTime.HasBottleneck(queueThreshold);
    }
}
