namespace PaceMind.Application.Coaching;

public sealed record CoachSession(string Day, string Type, int? DurationMinutes, string? IntensityZone);

public sealed record CoachWeekContext(
    string Sport,
    int WeekNumber,
    bool IsDraft,
    double TotalLoad,
    string GoalSummary,
    IReadOnlyList<CoachSession> Sessions);

public sealed record CoachChatTurn(bool FromCoach, string Content);

public interface ICoachService
{
    bool IsConfigured { get; }

    Task<string> CommentOnWeekAsync(CoachWeekContext week, CancellationToken cancellationToken = default);

    Task<string> ChatAsync(string goalSummary, IReadOnlyList<CoachChatTurn> history, CancellationToken cancellationToken = default);
}
