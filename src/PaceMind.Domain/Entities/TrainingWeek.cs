using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

/// <summary>
/// One week of the plan. <see cref="IsDraft"/> marks weeks that are still rough
/// outlines (they will be detailed/rewritten when they become the near week).
/// </summary>
public class TrainingWeek : BaseEntity
{
    public Guid TrainingPlanId { get; set; }
    public TrainingPlan TrainingPlan { get; set; } = null!;

    public int WeekNumber { get; set; }
    public DateOnly StartDate { get; set; }

    /// <summary>Short theme, e.g. "Base building" or "Sharpening".</summary>
    public string FocusTheme { get; set; } = string.Empty;

    /// <summary>True for far-out weeks kept as outlines until they come into focus.</summary>
    public bool IsDraft { get; set; } = true;

    public WeekStatus Status { get; set; } = WeekStatus.Upcoming;

    public ICollection<Workout> Workouts { get; set; } = [];
    public ICollection<AdaptationLog> AdaptationLogs { get; set; } = [];
}
