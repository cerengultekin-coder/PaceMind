using PaceMind.Domain.Enums;

namespace PaceMind.Contracts;

/// <summary>Onboarding input for a one-off, in-memory plan preview.</summary>
public sealed record PlanPreviewRequest(
    double TargetDistanceKm,
    int TargetTimeMinutes,
    DateOnly TargetDate,
    int DaysPerWeekMax,
    IReadOnlyList<DayOfWeek>? BlackoutDays = null,
    string? BaselineNote = null,
    Sport Sport = Sport.Running,
    DateOnly? StartDate = null);
