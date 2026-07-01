using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

public class Goal : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Sport Sport { get; set; } = Sport.Running;
    public TargetType TargetType { get; set; } = TargetType.DistanceTime;

    public double TargetDistanceKm { get; set; }

    public int TargetTimeMinutes { get; set; }

    public DateOnly TargetDate { get; set; }

    public string BaselineNote { get; set; } = string.Empty;

    public int DaysPerWeekMax { get; set; }

    public List<DayOfWeek> BlackoutDays { get; set; } = [];

    public GoalStatus Status { get; set; } = GoalStatus.Active;

    public TrainingPlan? Plan { get; set; }
}
