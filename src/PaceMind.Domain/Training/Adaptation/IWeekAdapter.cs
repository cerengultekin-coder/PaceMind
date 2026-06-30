using PaceMind.Domain.Entities;

namespace PaceMind.Domain.Training.Adaptation;

/// <summary>Rewrites the upcoming week from the feedback on the week just completed.</summary>
public interface IWeekAdapter
{
    /// <summary>
    /// Reads <paramref name="completedWeek"/>'s feedback and rescales <paramref name="nextWeek"/>'s
    /// sessions within safe bounds, recording an <see cref="AdaptationLog"/> on the rewritten week.
    /// </summary>
    AdaptationResult Adapt(
        TrainingWeek completedWeek,
        TrainingWeek nextWeek,
        ISportProfile profile,
        AdaptationOptions? options = null);
}
