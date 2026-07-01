namespace PaceMind.Contracts;

public sealed record CoachCommentRequest(string Sport, string GoalSummary, PlanWeekDto Week);

public sealed record CoachChatTurnDto(bool FromCoach, string Content);

public sealed record CoachChatRequest(string GoalSummary, IReadOnlyList<CoachChatTurnDto> History);

public sealed record CoachReplyResponse(string Reply, bool AiConfigured);
