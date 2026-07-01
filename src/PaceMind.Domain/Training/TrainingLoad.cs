using System.Globalization;
using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training;

public readonly record struct TrainingLoad : IComparable<TrainingLoad>
{
    public static readonly TrainingLoad Zero = default;

    public double Value { get; private init; }

    private TrainingLoad(double value) => Value = value;

    public static TrainingLoad FromScore(double score)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(score);
        return new TrainingLoad(score);
    }

    public static TrainingLoad ForSession(int durationMinutes, IntensityZone zone)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(durationMinutes);
        return new TrainingLoad(durationMinutes * zone.IntensityFactor());
    }

    public static TrainingLoad operator +(TrainingLoad left, TrainingLoad right) =>
        new(left.Value + right.Value);

    public int CompareTo(TrainingLoad other) => Value.CompareTo(other.Value);

    public static bool operator <(TrainingLoad left, TrainingLoad right) => left.Value < right.Value;
    public static bool operator >(TrainingLoad left, TrainingLoad right) => left.Value > right.Value;
    public static bool operator <=(TrainingLoad left, TrainingLoad right) => left.Value <= right.Value;
    public static bool operator >=(TrainingLoad left, TrainingLoad right) => left.Value >= right.Value;

    public override string ToString() => Value.ToString("F1", CultureInfo.InvariantCulture);
}
