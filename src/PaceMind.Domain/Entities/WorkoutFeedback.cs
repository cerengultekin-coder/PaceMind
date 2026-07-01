using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

public class WorkoutFeedback : BaseEntity
{
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;

    public FeedbackOutcome Outcome { get; set; }

    public Difficulty? Difficulty { get; set; }

    public SkipReason? SkipReason { get; set; }

    public string Note { get; set; } = string.Empty;
}
