using System.Text;
using Anthropic;
using Anthropic.Models.Messages;
using PaceMind.Application.Coaching;

namespace PaceMind.Infrastructure.Coaching;

public sealed class AnthropicCoachService : ICoachService
{
    private const string SystemPrompt =
        """
        You are PaceMind, a warm, concise endurance coach.
        The training plan's load is set by a deterministic rule engine — never invent paces,
        distances, or week-over-week load changes, and never contradict the numbers you are given.
        Explain the plan in plain language, answer questions, and stay encouraging and practical.
        Keep replies short (2-4 sentences) unless asked for more. Always write in English.
        """;

    private readonly AnthropicClient? _client;
    private readonly string _model;

    public AnthropicCoachService(AnthropicCoachOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _model = string.IsNullOrWhiteSpace(options.Model) ? "claude-opus-4-8" : options.Model;

        if (!string.IsNullOrWhiteSpace(options.ApiKey))
            _client = new AnthropicClient { ApiKey = options.ApiKey };
    }

    public bool IsConfigured => _client is not null;

    public Task<string> CommentOnWeekAsync(CoachWeekContext week, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(week);
        if (_client is null)
            return Task.FromResult("Connect an AI key to get a personalized take on this week.");

        var prompt = new StringBuilder();
        prompt.AppendLine($"Goal: {week.GoalSummary}");
        prompt.AppendLine($"Sport: {week.Sport}");
        prompt.AppendLine($"Week {week.WeekNumber}{(week.IsDraft ? " (draft)" : string.Empty)} — total training load {week.TotalLoad}.");
        prompt.AppendLine("Sessions:");
        foreach (var session in week.Sessions)
        {
            var duration = session.DurationMinutes is { } minutes ? $", {minutes} min" : string.Empty;
            var zone = session.IntensityZone is { } z ? $", {z}" : string.Empty;
            prompt.AppendLine($"- {session.Day}: {session.Type}{duration}{zone}");
        }

        prompt.Append("In 2-3 sentences, explain what this week is for and give one practical tip.");

        return SendAsync([new MessageParam { Role = Role.User, Content = prompt.ToString() }]);
    }

    public Task<string> ChatAsync(string goalSummary, IReadOnlyList<CoachChatTurn> history, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(history);
        if (_client is null)
            return Task.FromResult("The AI coach isn't connected yet. Add an Anthropic API key to chat.");
        if (history.Count == 0)
            return Task.FromResult("Ask me anything about your plan.");

        var messages = new List<MessageParam>(history.Count + 1)
        {
            new() { Role = Role.User, Content = $"For context, my goal is: {goalSummary}" },
        };
        foreach (var turn in history)
            messages.Add(new MessageParam { Role = turn.FromCoach ? Role.Assistant : Role.User, Content = turn.Content });

        return SendAsync(messages);
    }

    private async Task<string> SendAsync(IReadOnlyList<MessageParam> messages)
    {
        try
        {
            var response = await _client!.Messages.Create(new MessageCreateParams
            {
                Model = _model,
                MaxTokens = 1024,
                System = SystemPrompt,
                Messages = [.. messages],
            });

            var text = string.Concat(response.Content
                .Select(block => block.Value)
                .OfType<TextBlock>()
                .Select(block => block.Text));

            return string.IsNullOrWhiteSpace(text)
                ? "I couldn't generate a reply just now — please try again."
                : text.Trim();
        }
        catch (Exception)
        {
            return "The AI coach is unavailable right now. Please try again in a moment.";
        }
    }
}
