namespace PaceMind.Domain.Entities;

public class AdaptationLog : BaseEntity
{
    public Guid TrainingWeekId { get; set; }
    public TrainingWeek TrainingWeek { get; set; } = null!;

    public string TriggerReason { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;
}
