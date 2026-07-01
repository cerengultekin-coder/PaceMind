namespace PaceMind.Contracts;

public sealed record PlanAdaptResponse(
    PlanPreviewResponse Plan,
    AdaptationSummaryDto Adaptation);

public sealed record AdaptationSummaryDto(
    double AppliedFactor,
    string TriggerReason,
    string Summary,
    bool GoalAtRisk,
    int PlannedSessions,
    int Completed,
    int Skipped);
