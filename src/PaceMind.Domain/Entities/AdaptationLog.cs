namespace PaceMind.Domain.Entities;

/// <summary>
/// An audit trail of how and why a week was rewritten. Powers the transparency
/// that sets the product apart — the athlete can always see what changed and why.
/// </summary>
public class AdaptationLog : BaseEntity
{
    public Guid TrainingWeekId { get; set; }
    public TrainingWeek TrainingWeek { get; set; } = null!;

    /// <summary>What prompted the change, e.g. "3 sessions reported TooHard".</summary>
    public string TriggerReason { get; set; } = string.Empty;

    /// <summary>Plain-language summary of the adjustments made.</summary>
    public string Summary { get; set; } = string.Empty;
}
