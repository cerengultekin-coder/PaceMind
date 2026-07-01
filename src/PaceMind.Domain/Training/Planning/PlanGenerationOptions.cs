namespace PaceMind.Domain.Training.Planning;

public sealed record PlanGenerationOptions
{
    public static PlanGenerationOptions Default { get; } = new();

    public double StartingWeeklyLoad { get; init; } = 180;

    public double PeakWeeklyLoad { get; init; } = 360;

    public int RecoveryWeekInterval { get; init; } = 4;

    public double RecoveryLoadFactor { get; init; } = 0.70;

    public int TaperWeeks { get; init; } = 2;

    public void Validate()
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(StartingWeeklyLoad);
        if (PeakWeeklyLoad < StartingWeeklyLoad)
            throw new ArgumentException("Peak weekly load must be at least the starting weekly load.", nameof(PeakWeeklyLoad));
        ArgumentOutOfRangeException.ThrowIfNegative(RecoveryWeekInterval);
        if (RecoveryLoadFactor is <= 0 or > 1)
            throw new ArgumentException("Recovery load factor must be in the range (0, 1].", nameof(RecoveryLoadFactor));
        ArgumentOutOfRangeException.ThrowIfNegative(TaperWeeks);
    }
}
