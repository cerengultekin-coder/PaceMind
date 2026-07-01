using PaceMind.Domain.Entities;

namespace PaceMind.Domain.Training.Adaptation;

public interface IWeekAdapter
{
    AdaptationResult Adapt(
        TrainingWeek completedWeek,
        TrainingWeek nextWeek,
        ISportProfile profile,
        AdaptationOptions? options = null);
}
