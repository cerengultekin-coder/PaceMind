using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Entities;

/// <summary>
/// The athlete's signal after a session — the heart of the product. The weekly
/// adapter reads these to rewrite the upcoming week.
/// </summary>
public class WorkoutFeedback : BaseEntity
{
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;

    public FeedbackOutcome Outcome { get; set; }

    /// <summary>How it felt. Only meaningful when <see cref="Outcome"/> is Completed.</summary>
    public Difficulty? Difficulty { get; set; }

    /// <summary>Why it was missed. Only meaningful when <see cref="Outcome"/> is Skipped.</summary>
    public SkipReason? SkipReason { get; set; }

    public string Note { get; set; } = string.Empty;
}
