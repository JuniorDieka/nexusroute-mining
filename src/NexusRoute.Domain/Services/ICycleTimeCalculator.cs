using NexusRoute.Domain.Entities;
using NexusRoute.Domain.ValueObjects;

namespace NexusRoute.Domain.Services;

public interface ICycleTimeCalculator
{
    CycleTime? CalculateCycleTime(IEnumerable<OreMovementLog> cycleEvents);
    
    IEnumerable<CycleTime> CalculateCycleTimes(IEnumerable<OreMovementLog> logs, Guid assetId);
    
    TimeSpan CalculateAverageCycleTime(IEnumerable<CycleTime> cycles);
    
    bool DetectBottleneck(CycleTime cycleTime, TimeSpan queueThreshold);
}
