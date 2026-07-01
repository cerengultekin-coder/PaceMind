namespace PaceMind.Application.Coaching;

/// <summary>A single session as the coach sees it.</summary>
public sealed record CoachSession(string Day, string Type, int? DurationMinutes, string? IntensityZone);

/// <summary>Everything the coach needs to comment on one week.</summary>
public sealed record CoachWeekContext(
    string Sport,
    int WeekNumber,
    bool IsDraft,
    double TotalLoad,
    string GoalSummary,
    IReadOnlyList<CoachSession> Sessions);

/// <summary>One turn of the athlete-coach conversation.</summary>
public sealed record CoachChatTurn(bool FromCoach, string Content);

/// <summary>
/// The conversational, LLM-backed layer of the hybrid coach. It explains and discusses;
/// it never decides training load (the rule engine owns that). Implementations live in the
/// infrastructure layer so the API key and provider stay server-side.
/// </summary>
public interface ICoachService
{
    /// <summary>Whether a provider is configured. When false, callers get a friendly placeholder.</summary>
    bool IsConfigured { get; }

    /// <summary>A short, plain-language take on why a week looks the way it does.</summary>
    Task<string> CommentOnWeekAsync(CoachWeekContext week, CancellationToken cancellationToken = default);

    /// <summary>A coaching reply to the athlete's latest message, given the conversation so far.</summary>
    Task<string> ChatAsync(string goalSummary, IReadOnlyList<CoachChatTurn> history, CancellationToken cancellationToken = default);
}
