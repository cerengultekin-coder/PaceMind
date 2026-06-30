using System.Globalization;
using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training;

/// <summary>
/// The common currency of the coaching engine: a single, sport-agnostic score for how
/// taxing a session (or a week of sessions) is. Computing every sport down to this value
/// lets the adapter apply load rules — progression caps, fatigue detection — without
/// knowing which sport produced it.
/// </summary>
public readonly record struct TrainingLoad : IComparable<TrainingLoad>
{
    /// <summary>A session that costs nothing, e.g. a rest day.</summary>
    public static readonly TrainingLoad Zero = default;

    /// <summary>The raw load score. Always non-negative.</summary>
    public double Value { get; private init; }

    private TrainingLoad(double value) => Value = value;

    /// <summary>Wraps a pre-computed, non-negative load score.</summary>
    public static TrainingLoad FromScore(double score)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(score);
        return new TrainingLoad(score);
    }

    /// <summary>Derives the load of a single session from its duration and intensity zone.</summary>
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
