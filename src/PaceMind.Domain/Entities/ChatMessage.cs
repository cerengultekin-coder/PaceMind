using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

public class ChatMessage : BaseEntity
{
    public Guid TrainingPlanId { get; set; }
    public TrainingPlan TrainingPlan { get; set; } = null!;

    public ChatRole Role { get; set; }
    public required string Content { get; set; }
}
