using PaceMind.Domain.Entities;

namespace PaceMind.Domain.Training.Planning;

public interface IPlanGenerator
{
    TrainingPlan Generate(Goal goal, DateOnly startDate, PlanGenerationOptions? options = null);
}
