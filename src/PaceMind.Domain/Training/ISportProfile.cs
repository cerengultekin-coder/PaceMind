using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training;

public interface ISportProfile
{
    Sport Sport { get; }

    IReadOnlyCollection<WorkoutType> SupportedWorkoutTypes { get; }

    IntensityZone GetIntensityZone(WorkoutType workoutType);

    TrainingLoad EstimateLoad(WorkoutType workoutType, int durationMinutes) =>
        workoutType is WorkoutType.Rest
            ? TrainingLoad.Zero
            : TrainingLoad.ForSession(durationMinutes, GetIntensityZone(workoutType));
}
