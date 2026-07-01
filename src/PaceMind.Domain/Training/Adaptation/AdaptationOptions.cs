namespace PaceMind.Domain.Training.Adaptation;

public sealed record AdaptationOptions
{
    public static AdaptationOptions Default { get; } = new();

    public double MaxWeeklyIncreaseRate { get; init; } = 0.10;

    public double MaxWeeklyDecreaseRate { get; init; } = 0.20;

    public double HardSessionRatioThreshold { get; init; } = 0.5;

    public double EasySessionRatioThreshold { get; init; } = 0.5;

    public double StrongCompletionRate { get; init; } = 0.8;

    public double LowCompletionRate { get; init; } = 0.6;

    public double GoalRiskCompletionRate { get; init; } = 0.5;

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
