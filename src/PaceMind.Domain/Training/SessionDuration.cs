namespace PaceMind.Domain.Training;

/// <summary>Normalizes raw session minutes into athlete-friendly, bounded values.</summary>
internal static class SessionDuration
{
    public const int MinMinutes = 20;
    public const int RoundingMinutes = 5;

    public static int Round(double rawMinutes)
    {
        var rounded = (int)Math.Round(rawMinutes / RoundingMinutes) * RoundingMinutes;
        return Math.Max(MinMinutes, rounded);
    }
}
