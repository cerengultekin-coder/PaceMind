using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training;

/// <summary>
/// The training-science strategy for one sport. It is the single extension point that
/// keeps the planner and adapter sport-agnostic: supporting a new sport means adding a
/// profile, never touching the engine (Open/Closed). Implementations are stateless.
/// </summary>
public interface ISportProfile
{
    /// <summary>The sport this profile describes.</summary>
    Sport Sport { get; }

    /// <summary>The session types this sport offers, excluding <see cref="WorkoutType.Rest"/>.</summary>
    IReadOnlyCollection<WorkoutType> SupportedWorkoutTypes { get; }

    /// <summary>
    /// Maps a session type to its effort band for this sport.
    /// Throws for <see cref="WorkoutType.Rest"/> or any type the sport does not support.
    /// </summary>
    IntensityZone GetIntensityZone(WorkoutType workoutType);

    /// <summary>
    /// Estimates the training load of a planned session. Rest carries no load; every other
    /// type derives its load from duration and the sport's intensity mapping.
    /// </summary>
    TrainingLoad EstimateLoad(WorkoutType workoutType, int durationMinutes) =>
        workoutType is WorkoutType.Rest
            ? TrainingLoad.Zero
            : TrainingLoad.ForSession(durationMinutes, GetIntensityZone(workoutType));
}
