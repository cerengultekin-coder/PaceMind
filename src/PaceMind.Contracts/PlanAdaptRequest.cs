using PaceMind.Domain.Enums;

namespace PaceMind.Contracts;

public sealed record SessionFeedbackInput(
    DayOfWeek DayOfWeek,
    FeedbackOutcome Outcome,
    Difficulty? Difficulty = null,
    SkipReason? SkipReason = null);

public sealed record PlanAdaptRequest(
    PlanPreviewRequest Goal,
    DateOnly StartDate,
    IReadOnlyList<SessionFeedbackInput> Week1Feedback);
