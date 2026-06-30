using PaceMind.Domain.Entities;

namespace PaceMind.Domain.Training.Planning;

/// <summary>Builds an initial training plan for a goal from deterministic coaching rules.</summary>
public interface IPlanGenerator
{
    /// <summary>
    /// Produces a full plan from <paramref name="startDate"/> up to the goal date. Only the
    /// first week is concrete; later weeks are drafts the adapter rewrites as feedback arrives.
    /// </summary>
    TrainingPlan Generate(Goal goal, DateOnly startDate, PlanGenerationOptions? options = null);
}
