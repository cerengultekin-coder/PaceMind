using PaceMind.Domain.Entities;
using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training.Planning;

/// <summary>
/// Rule-based plan generator. It lays out the periodized load curve, fits sessions onto the
/// athlete's available days, and sizes each session from its intensity — all sport-agnostic,
/// delegating the sport-specific intensity mapping to the resolved <see cref="ISportProfile"/>.
/// </summary>
public sealed class PlanGenerator(ISportProfileResolver profileResolver) : IPlanGenerator
{
    public TrainingPlan Generate(Goal goal, DateOnly startDate, PlanGenerationOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(goal);
        var settings = options ?? PlanGenerationOptions.Default;
        settings.Validate();

        var profile = profileResolver.Resolve(goal.Sport);
        var totalWeeks = CountWeeks(startDate, goal.TargetDate);

        var blackoutDays = goal.BlackoutDays.ToHashSet();
        var availableDays = 7 - blackoutDays.Count;
        if (availableDays < 1)
            throw new ArgumentException("The goal blacks out every day of the week.", nameof(goal));
        if (goal.DaysPerWeekMax < 1)
            throw new ArgumentException("The goal must allow at least one training day per week.", nameof(goal));

        var trainingDaysPerWeek = Math.Min(goal.DaysPerWeekMax, availableDays);
        var trainingDays = TrainingDaySelector.Select(trainingDaysPerWeek, blackoutDays);
        var sessionTypes = WeeklySessionTemplate.ForDayCount(trainingDays.Count);
        var weeklyLoads = WeeklyLoadProgression.Build(totalWeeks, settings);

        var plan = new TrainingPlan { GoalId = goal.Id, Goal = goal };

        for (var weekIndex = 0; weekIndex < totalWeeks; weekIndex++)
        {
            var isFirstWeek = weekIndex == 0;
            var week = new TrainingWeek
            {
                TrainingPlanId = plan.Id,
                TrainingPlan = plan,
                WeekNumber = weekIndex + 1,
                StartDate = startDate.AddDays(7 * weekIndex),
                IsDraft = !isFirstWeek,
                Status = isFirstWeek ? WeekStatus.Active : WeekStatus.Upcoming,
            };

            AddWorkouts(week, trainingDays, sessionTypes, weeklyLoads[weekIndex], profile);
            plan.Weeks.Add(week);
        }

        return plan;
    }

    private void AddWorkouts(
        TrainingWeek week,
        IReadOnlyList<DayOfWeek> trainingDays,
        IReadOnlyList<WorkoutType> sessionTypes,
        TrainingLoad weeklyLoad,
        ISportProfile profile)
    {
        var typeByDay = new Dictionary<DayOfWeek, WorkoutType>(trainingDays.Count);
        for (var slot = 0; slot < trainingDays.Count; slot++)
            typeByDay[trainingDays[slot]] = sessionTypes[slot];

        var totalWeight = sessionTypes.Sum(WeeklySessionTemplate.LoadWeight);

        for (var dayOffset = 0; dayOffset < 7; dayOffset++)
        {
            var date = week.StartDate.AddDays(dayOffset);
            week.Workouts.Add(typeByDay.TryGetValue(date.DayOfWeek, out var type)
                ? BuildSession(week, date, type, weeklyLoad, totalWeight, profile)
                : BuildRestDay(week, date));
        }
    }

    private Workout BuildSession(
        TrainingWeek week,
        DateOnly date,
        WorkoutType type,
        TrainingLoad weeklyLoad,
        double totalWeight,
        ISportProfile profile)
    {
        var sessionLoad = weeklyLoad.Value * (WeeklySessionTemplate.LoadWeight(type) / totalWeight);
        var minutes = SessionDuration.Round(sessionLoad / profile.GetIntensityZone(type).IntensityFactor());

        return new Workout
        {
            TrainingWeekId = week.Id,
            TrainingWeek = week,
            ScheduledDate = date,
            DayOfWeek = date.DayOfWeek,
            Type = type,
            TargetDurationMinutes = minutes,
            Description = WorkoutNarrative.Describe(type, minutes),
            Status = WorkoutStatus.Planned,
        };
    }

    private static Workout BuildRestDay(TrainingWeek week, DateOnly date) => new()
    {
        TrainingWeekId = week.Id,
        TrainingWeek = week,
        ScheduledDate = date,
        DayOfWeek = date.DayOfWeek,
        Type = WorkoutType.Rest,
        Description = "Rest day",
        Status = WorkoutStatus.Planned,
    };

    private static int CountWeeks(DateOnly startDate, DateOnly targetDate)
    {
        var days = targetDate.DayNumber - startDate.DayNumber;
        if (days < 7)
            throw new ArgumentException("The goal date must be at least one week after the start date.", nameof(targetDate));

        return (int)Math.Ceiling(days / 7.0);
    }
}
