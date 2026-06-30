using System.Globalization;

namespace PaceMind.Domain.Training.Adaptation;

/// <summary>The decision the policy reached: how much to scale next week's load, and why.</summary>
internal readonly record struct LoadAdjustment(double Factor, string TriggerReason, string Summary);

/// <summary>
/// Turns a week's feedback into a bounded load adjustment. This is the deterministic core of
/// the adaptation: pain brakes hardest, then "too hard" and missed sessions ease the load,
/// while consistently easy, well-completed weeks earn a capped increase.
/// </summary>
internal static class LoadAdjustmentPolicy
{
    public static LoadAdjustment Decide(WeekFeedbackSummary summary, AdaptationOptions options)
    {
        if (summary.PlannedSessions == 0 || summary.FeedbackCount == 0)
            return new LoadAdjustment(1.0, "No feedback recorded", "Plan left unchanged — no sessions were logged.");

        double factor;
        string trigger;

        if (summary.HadPain)
        {
            factor = options.PainAdjustmentFactor;
            trigger = "Pain reported during a session";
        }
        else
        {
            var hardRatio = summary.Completed == 0 ? 0 : (double)summary.TooHard / summary.Completed;
            var easyRatio = summary.Completed == 0 ? 0 : (double)summary.Easy / summary.Completed;

            if (hardRatio >= options.HardSessionRatioThreshold)
            {
                factor = 1 - options.MaxWeeklyDecreaseRate / 2;
                trigger = $"{summary.TooHard} of {summary.Completed} sessions felt too hard";
            }
            else if (easyRatio >= options.EasySessionRatioThreshold && summary.CompletionRate >= options.StrongCompletionRate)
            {
                factor = 1 + options.MaxWeeklyIncreaseRate;
                trigger = $"{summary.Easy} of {summary.Completed} sessions felt easy";
            }
            else
            {
                factor = 1.0;
                trigger = "Sessions felt about right";
            }

            if (summary.CompletionRate < options.LowCompletionRate)
            {
                factor = Math.Min(factor, 1 - options.MaxWeeklyDecreaseRate / 2);
                trigger = $"Only {summary.Completed} of {summary.PlannedSessions} sessions completed";
            }
        }

        factor = Math.Clamp(factor, 1 - options.MaxWeeklyDecreaseRate, 1 + options.MaxWeeklyIncreaseRate);
        return new LoadAdjustment(factor, trigger, BuildSummary(factor));
    }

    private static string BuildSummary(double factor)
    {
        var percent = Math.Round(Math.Abs(factor - 1) * 100).ToString(CultureInfo.InvariantCulture);
        return factor switch
        {
            > 1 => $"Increased next week's load by {percent}%.",
            < 1 => $"Reduced next week's load by {percent}%.",
            _ => "Kept next week's load steady.",
        };
    }
}
