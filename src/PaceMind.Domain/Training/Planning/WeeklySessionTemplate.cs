using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training.Planning;

/// <summary>
/// Chooses the mix of session types for a week given how many training days are available,
/// and the relative share of weekly load each type should carry. The template is ordered so
/// that, mapped onto calendar-ordered days, quality work lands midweek and the long day last.
/// </summary>
internal static class WeeklySessionTemplate
{
    public static IReadOnlyList<WorkoutType> ForDayCount(int days) => days switch
    {
        1 => [WorkoutType.Long],
        2 => [WorkoutType.Easy, WorkoutType.Long],
        3 => [WorkoutType.Easy, WorkoutType.Tempo, WorkoutType.Long],
        4 => [WorkoutType.Easy, WorkoutType.Tempo, WorkoutType.Easy, WorkoutType.Long],
        5 => [WorkoutType.Easy, WorkoutType.Tempo, WorkoutType.Easy, WorkoutType.Interval, WorkoutType.Long],
        6 => [WorkoutType.Easy, WorkoutType.Tempo, WorkoutType.Easy, WorkoutType.Interval, WorkoutType.Easy, WorkoutType.Long],
        7 => [WorkoutType.Easy, WorkoutType.Tempo, WorkoutType.Easy, WorkoutType.Interval, WorkoutType.Easy, WorkoutType.Easy, WorkoutType.Long],
        _ => throw new ArgumentOutOfRangeException(nameof(days), days, "Training days per week must be between 1 and 7."),
    };

    /// <summary>Relative weight used to split a week's load across its sessions.</summary>
    public static double LoadWeight(WorkoutType type) => type switch
    {
        WorkoutType.Long => 1.6,
        WorkoutType.Tempo => 1.2,
        WorkoutType.Interval => 1.1,
        WorkoutType.Easy => 1.0,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Type carries no planned load."),
    };
}
