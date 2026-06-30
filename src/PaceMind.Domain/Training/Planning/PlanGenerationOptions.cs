namespace PaceMind.Domain.Training.Planning;

/// <summary>
/// Tunable inputs for periodization. Defaults encode a conservative, generic build:
/// a steady ramp to peak with periodic recovery weeks and a taper into the goal date.
/// The weekly adapter refines these numbers per athlete as feedback arrives.
/// </summary>
public sealed record PlanGenerationOptions
{
    public static PlanGenerationOptions Default { get; } = new();

    /// <summary>Weekly training load the plan opens with.</summary>
    public double StartingWeeklyLoad { get; init; } = 180;

    /// <summary>Weekly training load targeted at the peak of the build phase.</summary>
    public double PeakWeeklyLoad { get; init; } = 360;

    /// <summary>Every Nth week is a reduced-load recovery week. Zero disables recovery weeks.</summary>
    public int RecoveryWeekInterval { get; init; } = 4;

    /// <summary>Multiplier applied to a recovery week's load.</summary>
    public double RecoveryLoadFactor { get; init; } = 0.70;

    /// <summary>Number of reduced-load weeks leading into the goal date.</summary>
    public int TaperWeeks { get; init; } = 2;

    /// <summary>Throws <see cref="ArgumentException"/> if any option is out of range.</summary>
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
