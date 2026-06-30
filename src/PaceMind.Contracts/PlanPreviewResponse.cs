using PaceMind.Domain.Enums;

namespace PaceMind.Contracts;

/// <summary>A generated plan flattened for display, free of entity navigation cycles.</summary>
public sealed record PlanPreviewResponse(
    Sport Sport,
    DateOnly StartDate,
    DateOnly TargetDate,
    int TotalWeeks,
    IReadOnlyList<PlanWeekDto> Weeks);

public sealed record PlanWeekDto(
    int WeekNumber,
    DateOnly StartDate,
    bool IsDraft,
    string Status,
    double TotalLoad,
    IReadOnlyList<PlanWorkoutDto> Workouts);

public sealed record PlanWorkoutDto(
    DateOnly Date,
    DayOfWeek DayOfWeek,
    string Type,
    int? DurationMinutes,
    string? IntensityZone,
    double Load,
    string Description);
