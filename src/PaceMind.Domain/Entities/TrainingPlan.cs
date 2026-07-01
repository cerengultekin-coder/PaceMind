using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

public class TrainingPlan : BaseEntity
{
    public Guid GoalId { get; set; }
    public Goal Goal { get; set; } = null!;

    public GoalStatus Status { get; set; } = GoalStatus.Active;

    public ICollection<TrainingWeek> Weeks { get; set; } = [];
    public ICollection<ChatMessage> ChatMessages { get; set; } = [];
}
