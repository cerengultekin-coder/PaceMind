namespace PaceMind.Domain.Training.Planning;

/// <summary>
/// Picks which weekdays carry sessions. Days are chosen by a fixed priority that spreads
/// work across the week (so hard days are not back-to-back), then returned in calendar order.
/// </summary>
internal static class TrainingDaySelector
{
    private static readonly DayOfWeek[] SpreadPriority =
    [
        DayOfWeek.Tuesday,
        DayOfWeek.Saturday,
        DayOfWeek.Thursday,
        DayOfWeek.Sunday,
        DayOfWeek.Monday,
        DayOfWeek.Wednesday,
        DayOfWeek.Friday,
    ];

    public static IReadOnlyList<DayOfWeek> Select(int maxDays, IReadOnlySet<DayOfWeek> blackoutDays) =>
        SpreadPriority
            .Where(day => !blackoutDays.Contains(day))
            .Take(maxDays)
            .OrderBy(MondayFirstIndex)
            .ToArray();

    private static int MondayFirstIndex(DayOfWeek day) => ((int)day + 6) % 7;
}
