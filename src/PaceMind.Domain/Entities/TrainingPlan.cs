using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

/// <summary>
/// The living plan for a goal. The near week is concrete; later weeks stay drafts
/// because they will be rewritten as feedback arrives.
/// </summary>
public class TrainingPlan : BaseEntity
{
    public Guid GoalId { get; set; }
    public Goal Goal { get; set; } = null!;

    public GoalStatus Status { get; set; } = GoalStatus.Active;

    public ICollection<TrainingWeek> Weeks { get; set; } = [];
    public ICollection<ChatMessage> ChatMessages { get; set; } = [];
}
