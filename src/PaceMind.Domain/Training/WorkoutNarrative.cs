using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training;

internal static class WorkoutNarrative
{
    public static string Describe(WorkoutType type, int minutes) => $"{minutes}-minute {Label(type)} session";

    public static string Label(WorkoutType type) => type switch
    {
        WorkoutType.Easy => "easy",
        WorkoutType.Tempo => "tempo",
        WorkoutType.Interval => "interval",
        WorkoutType.Long => "long",
        WorkoutType.Rest => "rest",
        _ => type.ToString().ToLowerInvariant(),
    };
}
