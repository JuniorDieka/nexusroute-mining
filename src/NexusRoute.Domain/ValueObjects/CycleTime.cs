namespace NexusRoute.Domain.ValueObjects;

public record CycleTime
{
    public TimeSpan LoadTime { get; init; }
    public TimeSpan HaulTime { get; init; }
    public TimeSpan DumpTime { get; init; }
    public TimeSpan ReturnTime { get; init; }
    public TimeSpan QueueTime { get; init; }

    public TimeSpan TotalTime => LoadTime + HaulTime + DumpTime + ReturnTime + QueueTime;
    public TimeSpan ProductiveTime => LoadTime + HaulTime + DumpTime + ReturnTime;

    public CycleTime(
        TimeSpan loadTime,
        TimeSpan haulTime,
        TimeSpan dumpTime,
        TimeSpan returnTime,
        TimeSpan queueTime)
    {
        if (loadTime < TimeSpan.Zero)
            throw new ArgumentException("Load time cannot be negative.", nameof(loadTime));
        if (haulTime < TimeSpan.Zero)
            throw new ArgumentException("Haul time cannot be negative.", nameof(haulTime));
        if (dumpTime < TimeSpan.Zero)
            throw new ArgumentException("Dump time cannot be negative.", nameof(dumpTime));
        if (returnTime < TimeSpan.Zero)
            throw new ArgumentException("Return time cannot be negative.", nameof(returnTime));
        if (queueTime < TimeSpan.Zero)
            throw new ArgumentException("Queue time cannot be negative.", nameof(queueTime));

        LoadTime = loadTime;
        HaulTime = haulTime;
        DumpTime = dumpTime;
        ReturnTime = returnTime;
        QueueTime = queueTime;
    }

    public double EfficiencyPercentage => TotalTime.TotalSeconds > 0
        ? (ProductiveTime.TotalSeconds / TotalTime.TotalSeconds) * 100
        : 0;

    public bool HasBottleneck(TimeSpan threshold) => QueueTime > threshold;
}
