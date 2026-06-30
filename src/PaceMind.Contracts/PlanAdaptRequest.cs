using PaceMind.Domain.Enums;

namespace PaceMind.Contracts;

/// <summary>Feedback for a single completed session, keyed by its weekday.</summary>
public sealed record SessionFeedbackInput(
    DayOfWeek DayOfWeek,
    FeedbackOutcome Outcome,
    Difficulty? Difficulty = null,
    SkipReason? SkipReason = null);

/// <summary>
/// Stateless adaptation request: the original goal and start date (to regenerate the plan)
/// plus the athlete's feedback for the first week. The API replays generation, applies the
/// feedback, and adapts the following week.
/// </summary>
public sealed record PlanAdaptRequest(
    PlanPreviewRequest Goal,
    DateOnly StartDate,
    IReadOnlyList<SessionFeedbackInput> Week1Feedback);
