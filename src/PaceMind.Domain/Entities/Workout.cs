using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

/// <summary>
/// A single planned session. <see cref="Rationale"/> captures *why* the coach chose
/// it, so the plan stays explainable to the athlete.
/// </summary>
public class Workout : BaseEntity
{
    public Guid TrainingWeekId { get; set; }
    public TrainingWeek TrainingWeek { get; set; } = null!;

    public DateOnly ScheduledDate { get; set; }
    public DayOfWeek DayOfWeek { get; set; }

    public WorkoutType Type { get; set; }

    /// <summary>Target distance in km. Null for rest days or time-based sessions.</summary>
    public double? TargetDistanceKm { get; set; }

    /// <summary>Target duration in minutes. Null for rest days.</summary>
    public int? TargetDurationMinutes { get; set; }

    /// <summary>Human-readable prescription, e.g. "6 x 400m @ 5K pace, 90s jog recovery".</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Why this session is here this week — shown to the athlete on request.</summary>
    public string Rationale { get; set; } = string.Empty;

    public WorkoutStatus Status { get; set; } = WorkoutStatus.Planned;

    public ICollection<WorkoutFeedback> Feedback { get; set; } = [];

    /// <summary>v2: populated from an uploaded GPX/FIT file. Always null in the MVP.</summary>
    public ActivityData? ActivityData { get; set; }
}
