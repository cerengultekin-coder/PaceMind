using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training;

/// <summary>
/// Maps each <see cref="IntensityZone"/> to its relative physiological cost, the
/// multiplier that turns session duration into a comparable training load.
/// </summary>
public static class IntensityZoneExtensions
{
    /// <summary>
    /// Relative effort weight of the zone. Calibrated so an hour at <see cref="IntensityZone.Threshold"/>
    /// equals one "load unit" per minute; lighter zones cost less, anaerobic work costs more.
    /// </summary>
    public static double IntensityFactor(this IntensityZone zone) => zone switch
    {
        IntensityZone.Recovery => 0.50,
        IntensityZone.Easy => 0.70,
        IntensityZone.Moderate => 0.85,
        IntensityZone.Threshold => 1.00,
        IntensityZone.Anaerobic => 1.25,
        _ => throw new ArgumentOutOfRangeException(nameof(zone), zone, "Unknown intensity zone."),
    };
}
