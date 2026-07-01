namespace PaceMind.Contracts;

/// <summary>Asks the coach to comment on a specific week of the current plan.</summary>
public sealed record CoachCommentRequest(string Sport, string GoalSummary, PlanWeekDto Week);

/// <summary>One turn of the athlete-coach conversation.</summary>
public sealed record CoachChatTurnDto(bool FromCoach, string Content);

/// <summary>Sends the conversation so far and asks for the coach's next reply.</summary>
public sealed record CoachChatRequest(string GoalSummary, IReadOnlyList<CoachChatTurnDto> History);

/// <summary>A coach reply, plus whether a live AI provider is actually wired up.</summary>
public sealed record CoachReplyResponse(string Reply, bool AiConfigured);
