using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

/// <summary>
/// What the athlete wants to achieve, plus the constraints the plan must respect.
/// Produced by onboarding; consumed by the plan generator and weekly adapter.
/// </summary>
public class Goal : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Sport Sport { get; set; } = Sport.Running;
    public TargetType TargetType { get; set; } = TargetType.DistanceTime;

    /// <summary>Target race/effort distance, e.g. 10.0 km.</summary>
    public double TargetDistanceKm { get; set; }

    /// <summary>Goal finishing time in minutes, e.g. 50.</summary>
    public int TargetTimeMinutes { get; set; }

    /// <summary>When the athlete wants to hit the goal.</summary>
    public DateOnly TargetDate { get; set; }

    /// <summary>Free-text current fitness, e.g. "runs 2-3x/week, longest 5K".</summary>
    public string BaselineNote { get; set; } = string.Empty;

    /// <summary>Maximum training days the athlete can commit to per week.</summary>
    public int DaysPerWeekMax { get; set; }

    /// <summary>Days the athlete is never available (e.g. [Sunday]).</summary>
    public List<DayOfWeek> BlackoutDays { get; set; } = [];

    public GoalStatus Status { get; set; } = GoalStatus.Active;

    /// <summary>The single active plan generated for this goal.</summary>
    public TrainingPlan? Plan { get; set; }
}
