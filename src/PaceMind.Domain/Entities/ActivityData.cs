using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

public class ActivityData : BaseEntity
{
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;

    public ActivitySource Source { get; set; } = ActivitySource.Manual;

    public double DistanceKm { get; set; }
    public int DurationMinutes { get; set; }

    public int? AvgPaceSecPerKm { get; set; }
    public int? AvgHeartRate { get; set; }
    public double? ElevationGainM { get; set; }

    public string? RawFileRef { get; set; }
}
