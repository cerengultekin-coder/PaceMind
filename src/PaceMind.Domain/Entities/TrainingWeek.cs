using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

public class TrainingWeek : BaseEntity
{
    public Guid TrainingPlanId { get; set; }
    public TrainingPlan TrainingPlan { get; set; } = null!;

    public int WeekNumber { get; set; }
    public DateOnly StartDate { get; set; }

    public string FocusTheme { get; set; } = string.Empty;

    public bool IsDraft { get; set; } = true;

    public WeekStatus Status { get; set; } = WeekStatus.Upcoming;

    public ICollection<Workout> Workouts { get; set; } = [];
    public ICollection<AdaptationLog> AdaptationLogs { get; set; } = [];
}
