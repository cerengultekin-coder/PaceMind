namespace PaceMind.Domain.Training.Planning;

/// <summary>
/// Builds the per-week target load curve: a linear ramp from start to peak across the
/// build phase, dipped on recovery weeks, then tapered down into the goal date.
/// </summary>
internal static class WeeklyLoadProgression
{
    public static IReadOnlyList<TrainingLoad> Build(int totalWeeks, PlanGenerationOptions options)
    {
        // Always keep at least one build week, even when the taper would otherwise consume the plan.
        var taperWeeks = Math.Min(options.TaperWeeks, totalWeeks - 1);
        var buildWeeks = totalWeeks - taperWeeks;
        var loads = new TrainingLoad[totalWeeks];

        for (var week = 0; week < buildWeeks; week++)
        {
            var progress = buildWeeks == 1 ? 1d : (double)week / (buildWeeks - 1);
            var load = options.StartingWeeklyLoad +
                       (options.PeakWeeklyLoad - options.StartingWeeklyLoad) * progress;

            if (options.RecoveryWeekInterval > 0 && (week + 1) % options.RecoveryWeekInterval == 0)
                load *= options.RecoveryLoadFactor;

            loads[week] = TrainingLoad.FromScore(load);
        }

        // Taper: descend from 60% to 40% of peak, leaving the goal week the lightest.
        for (var taper = 0; taper < taperWeeks; taper++)
        {
            var progress = taperWeeks == 1 ? 1d : (double)taper / (taperWeeks - 1);
            var factor = 0.60 - 0.20 * progress;
            loads[buildWeeks + taper] = TrainingLoad.FromScore(options.PeakWeeklyLoad * factor);
        }

        return loads;
    }
}
