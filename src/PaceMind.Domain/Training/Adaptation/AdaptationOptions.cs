namespace PaceMind.Domain.Training.Adaptation;

/// <summary>
/// Safety bounds and thresholds the adapter works within. Defaults follow common coaching
/// guidance — notably the "~10% per week" progression cap — so the LLM/UX layers can never
/// push an unsafe jump in load.
/// </summary>
public sealed record AdaptationOptions
{
    public static AdaptationOptions Default { get; } = new();

    /// <summary>Largest week-over-week load increase, as a fraction.</summary>
    public double MaxWeeklyIncreaseRate { get; init; } = 0.10;

    /// <summary>Largest week-over-week load decrease, as a fraction.</summary>
    public double MaxWeeklyDecreaseRate { get; init; } = 0.20;

    /// <summary>Share of completed sessions feeling too hard that triggers easing the load.</summary>
    public double HardSessionRatioThreshold { get; init; } = 0.5;

    /// <summary>Share of completed sessions feeling easy that allows adding load.</summary>
    public double EasySessionRatioThreshold { get; init; } = 0.5;

    /// <summary>Completion rate required before load is increased.</summary>
    public double StrongCompletionRate { get; init; } = 0.8;

    /// <summary>Completion rate below which the load is eased regardless of difficulty.</summary>
    public double LowCompletionRate { get; init; } = 0.6;

    /// <summary>Completion rate below which the goal is flagged as at risk.</summary>
    public double GoalRiskCompletionRate { get; init; } = 0.5;

    /// <summary>Load multiplier applied when pain is reported (a hard safety brake).</summary>
    public double PainAdjustmentFactor { get; init; } = 0.80;

    public void Validate()
    {
        ArgumentOutOfRangeException.ThrowIfNegative(MaxWeeklyIncreaseRate);
        if (MaxWeeklyDecreaseRate is < 0 or >= 1)
            throw new ArgumentException("Max weekly decrease must be in the range [0, 1).", nameof(MaxWeeklyDecreaseRate));
        if (PainAdjustmentFactor < 1 - MaxWeeklyDecreaseRate || PainAdjustmentFactor > 1)
            throw new ArgumentException("Pain adjustment factor must stay within the decrease bound.", nameof(PainAdjustmentFactor));
    }
}
