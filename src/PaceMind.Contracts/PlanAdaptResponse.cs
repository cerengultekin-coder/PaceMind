namespace PaceMind.Contracts;

/// <summary>The adapted plan plus a human-readable account of what changed and why.</summary>
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
