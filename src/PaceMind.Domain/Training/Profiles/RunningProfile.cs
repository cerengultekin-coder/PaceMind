using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training.Profiles;

public sealed class RunningProfile : ISportProfile
{
    public Sport Sport => Sport.Running;

    public IReadOnlyCollection<WorkoutType> SupportedWorkoutTypes { get; } =
    [
        WorkoutType.Easy,
        WorkoutType.Tempo,
        WorkoutType.Interval,
        WorkoutType.Long,
    ];

    public IntensityZone GetIntensityZone(WorkoutType workoutType) => workoutType switch
    {
        WorkoutType.Easy => IntensityZone.Easy,
        WorkoutType.Long => IntensityZone.Moderate,
        WorkoutType.Tempo => IntensityZone.Threshold,
        WorkoutType.Interval => IntensityZone.Anaerobic,
        _ => throw new ArgumentOutOfRangeException(
            nameof(workoutType), workoutType,
            $"Workout type '{workoutType}' is not supported by {nameof(RunningProfile)}."),
    };
}
