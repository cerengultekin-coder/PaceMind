using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training;

public static class IntensityZoneExtensions
{
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
