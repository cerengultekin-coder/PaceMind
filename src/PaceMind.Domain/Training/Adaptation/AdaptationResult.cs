using PaceMind.Domain.Entities;

namespace PaceMind.Domain.Training.Adaptation;

/// <summary>The outcome of adapting one week: the load change applied, the feedback it was
/// based on, whether the goal is now at risk, and the audit log written to the rewritten week.</summary>
public sealed record AdaptationResult(
    double AppliedFactor,
    WeekFeedbackSummary Summary,
    bool GoalAtRisk,
    AdaptationLog Log);
