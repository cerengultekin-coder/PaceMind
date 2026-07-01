using PaceMind.Contracts;
using PaceMind.Domain.Entities;
using PaceMind.Domain.Enums;
using PaceMind.Domain.Training;

namespace PaceMind.Api.Contracts;

internal static class PlanPreviewMapper
{
    public static PlanPreviewResponse ToResponse(TrainingPlan plan, Goal goal, DateOnly startDate, ISportProfile profile)
    {
        var weeks = plan.Weeks
            .OrderBy(week => week.WeekNumber)
            .Select(week => ToWeek(week, profile))
            .ToList();

        return new PlanPreviewResponse(goal.Sport, startDate, goal.TargetDate, weeks.Count, weeks);
    }

    internal static PlanWeekDto ToWeek(TrainingWeek week, ISportProfile profile)
    {
        var workouts = week.Workouts
            .OrderBy(workout => workout.ScheduledDate)
            .Select(workout => ToWorkout(workout, profile))
            .ToList();

        return new PlanWeekDto(
            week.WeekNumber,
            week.StartDate,
            week.IsDraft,
            week.Status.ToString(),
            Math.Round(workouts.Sum(workout => workout.Load), 1),
            workouts);
    }

    private static PlanWorkoutDto ToWorkout(Workout workout, ISportProfile profile)
    {
        if (workout.Type is WorkoutType.Rest)
            return new PlanWorkoutDto(workout.ScheduledDate, workout.DayOfWeek, nameof(WorkoutType.Rest), null, null, 0, workout.Description);

        var zone = profile.GetIntensityZone(workout.Type);
        var load = profile.EstimateLoad(workout.Type, workout.TargetDurationMinutes ?? 0).Value;

        return new PlanWorkoutDto(
            workout.ScheduledDate,
            workout.DayOfWeek,
            workout.Type.ToString(),
            workout.TargetDurationMinutes,
            zone.ToString(),
            Math.Round(load, 1),
            workout.Description);
    }
}
