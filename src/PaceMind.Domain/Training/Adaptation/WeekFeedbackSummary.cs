namespace PaceMind.Domain.Training.Adaptation;

public sealed record WeekFeedbackSummary(
    int PlannedSessions,
    int Completed,
    int Skipped,
    int TooHard,
    int JustRight,
    int Easy,
    bool HadPain)
{
    public int FeedbackCount => Completed + Skipped;

    public double CompletionRate => PlannedSessions == 0 ? 0 : (double)Completed / PlannedSessions;
}
