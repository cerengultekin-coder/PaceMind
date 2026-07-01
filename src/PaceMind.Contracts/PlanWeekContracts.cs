namespace PaceMind.Contracts;

public sealed record PlanWeekRequest(
    PlanPreviewRequest Goal,
    int WeekNumber,
    DateOnly? StartDate = null,
    IReadOnlyList<SessionFeedbackInput>? PreviousFeedback = null);

public sealed record PlanWeekResponse(
    PlanWeekDto Week,
    int TotalWeeks,
    DateOnly StartDate,
    string? AdaptationSummary,
    bool GoalAtRisk);
