using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

/// <summary>
/// v2: objective metrics for a session, parsed from an uploaded GPX/FIT file.
/// The table exists now so the schema is ready, but the MVP never writes to it.
/// </summary>
public class ActivityData : BaseEntity
{
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;

    public ActivitySource Source { get; set; } = ActivitySource.Manual;

    public double DistanceKm { get; set; }
    public int DurationMinutes { get; set; }

    /// <summary>Average pace in seconds per kilometre.</summary>
    public int? AvgPaceSecPerKm { get; set; }
    public int? AvgHeartRate { get; set; }
    public double? ElevationGainM { get; set; }

    /// <summary>Reference (blob key / path) to the original uploaded file.</summary>
    public string? RawFileRef { get; set; }
}
