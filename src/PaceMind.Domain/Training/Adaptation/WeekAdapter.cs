using PaceMind.Domain.Entities;
using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training.Adaptation;

public sealed class WeekAdapter : IWeekAdapter
{
    public AdaptationResult Adapt(
        TrainingWeek completedWeek,
        TrainingWeek nextWeek,
        ISportProfile profile,
        AdaptationOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(completedWeek);
        ArgumentNullException.ThrowIfNull(nextWeek);
        ArgumentNullException.ThrowIfNull(profile);

        var settings = options ?? AdaptationOptions.Default;
        settings.Validate();

        var summary = WeekFeedbackAnalyzer.Analyze(completedWeek);
        var adjustment = LoadAdjustmentPolicy.Decide(summary, settings);

        Rescale(nextWeek, adjustment.Factor);
        nextWeek.IsDraft = false;

        var log = new AdaptationLog
        {
            TrainingWeekId = nextWeek.Id,
            TrainingWeek = nextWeek,
            TriggerReason = adjustment.TriggerReason,
            Summary = adjustment.Summary,
        };
        nextWeek.AdaptationLogs.Add(log);

        var goalAtRisk = summary.FeedbackCount > 0
                         && (summary.CompletionRate < settings.GoalRiskCompletionRate || summary.HadPain);

        return new AdaptationResult(adjustment.Factor, summary, goalAtRisk, log);
    }

    private static void Rescale(TrainingWeek week, double factor)
    {
        foreach (var workout in week.Workouts)
        {
            if (workout.Type is WorkoutType.Rest || workout.TargetDurationMinutes is not { } minutes)
                continue;

            var adjusted = SessionDuration.Round(minutes * factor);
            workout.TargetDurationMinutes = adjusted;
            workout.Description = WorkoutNarrative.Describe(workout.Type, adjusted);
        }
    }
}
