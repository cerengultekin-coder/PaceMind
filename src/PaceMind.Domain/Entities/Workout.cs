using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

public class Workout : BaseEntity
{
    public Guid TrainingWeekId { get; set; }
    public TrainingWeek TrainingWeek { get; set; } = null!;

    public DateOnly ScheduledDate { get; set; }
    public DayOfWeek DayOfWeek { get; set; }

    public WorkoutType Type { get; set; }

    public double? TargetDistanceKm { get; set; }

    public int? TargetDurationMinutes { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Rationale { get; set; } = string.Empty;

    public WorkoutStatus Status { get; set; } = WorkoutStatus.Planned;

    public ICollection<WorkoutFeedback> Feedback { get; set; } = [];

    public ActivityData? ActivityData { get; set; }
}
