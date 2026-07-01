using PaceMind.Domain.Entities;

namespace PaceMind.Domain.Training.Adaptation;

public sealed record AdaptationResult(
    double AppliedFactor,
    WeekFeedbackSummary Summary,
    bool GoalAtRisk,
    AdaptationLog Log);
