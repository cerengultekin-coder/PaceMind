using PaceMind.Domain.Enums;

namespace PaceMind.Contracts;

public sealed record PlanPreviewRequest(
    double TargetDistanceKm,
    int TargetTimeMinutes,
    DateOnly TargetDate,
    int DaysPerWeekMax,
    IReadOnlyList<DayOfWeek>? BlackoutDays = null,
    string? BaselineNote = null,
    Sport Sport = Sport.Running,
    DateOnly? StartDate = null);
