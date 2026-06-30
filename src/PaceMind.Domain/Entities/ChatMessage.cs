using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

/// <summary>
/// One turn in the athlete-coach conversation. Gives the plan a voice the athlete
/// can question ("why is this week so light?", "my knee hurts").
/// </summary>
public class ChatMessage : BaseEntity
{
    public Guid TrainingPlanId { get; set; }
    public TrainingPlan TrainingPlan { get; set; } = null!;

    public ChatRole Role { get; set; }
    public required string Content { get; set; }
}
