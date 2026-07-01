using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using PaceMind.Application.Coaching;

namespace PaceMind.Infrastructure.Coaching;

public sealed class GeminiCoachService : ICoachService
{
    private const string SystemPrompt =
        """
        You are PaceMind, a warm, concise endurance coach.
        The training plan's load is set by a deterministic rule engine — never invent paces,
        distances, or week-over-week load changes, and never contradict the numbers you are given.
        Explain the plan in plain language, answer questions, and stay encouraging and practical.
        Keep replies short (2-4 sentences) unless asked for more. Always write in English.
        """;

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _http;
    private readonly string? _apiKey;
    private readonly string _model;

    public GeminiCoachService(GeminiCoachOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _apiKey = string.IsNullOrWhiteSpace(options.ApiKey) ? null : options.ApiKey;
        _model = string.IsNullOrWhiteSpace(options.Model) ? "gemini-2.5-flash" : options.Model;
        _http = new HttpClient { BaseAddress = new Uri("https://generativelanguage.googleapis.com/") };
    }

    public bool IsConfigured => _apiKey is not null;

    public Task<string> CommentOnWeekAsync(CoachWeekContext week, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(week);
        if (_apiKey is null)
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

        return SendAsync([new GeminiContent("user", [new GeminiPart(prompt.ToString())])], cancellationToken);
    }

    public Task<string> ChatAsync(string goalSummary, IReadOnlyList<CoachChatTurn> history, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(history);
        if (_apiKey is null)
            return Task.FromResult("The AI coach isn't connected yet. Add a Gemini API key to chat.");
        if (history.Count == 0)
            return Task.FromResult("Ask me anything about your plan.");

        var contents = new List<GeminiContent>(history.Count + 1)
        {
            new("user", [new GeminiPart($"For context, my goal is: {goalSummary}")]),
        };
        foreach (var turn in history)
            contents.Add(new GeminiContent(turn.FromCoach ? "model" : "user", [new GeminiPart(turn.Content)]));

        return SendAsync(contents, cancellationToken);
    }

    private async Task<string> SendAsync(IReadOnlyList<GeminiContent> contents, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GeminiRequest(
                new GeminiSystemInstruction([new GeminiPart(SystemPrompt)]),
                contents,
                new GeminiGenerationConfig(1024, 0.7));

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"v1beta/models/{_model}:generateContent")
            {
                Content = JsonContent.Create(request, options: JsonOptions),
            };
            httpRequest.Headers.Add("x-goog-api-key", _apiKey);

            using var response = await _http.SendAsync(httpRequest, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return "The AI coach is unavailable right now. Please try again in a moment.";

            var payload = await response.Content.ReadFromJsonAsync<GeminiResponse>(JsonOptions, cancellationToken);
            var parts = payload?.Candidates is [var candidate, ..] ? candidate.Content?.Parts : null;
            var text = parts is null ? null : string.Concat(parts.Select(part => part.Text));

            return string.IsNullOrWhiteSpace(text)
                ? "I couldn't generate a reply just now — please try again."
                : text.Trim();
        }
        catch (Exception)
        {
            return "The AI coach is unavailable right now. Please try again in a moment.";
        }
    }

    private sealed record GeminiPart(string Text);
    private sealed record GeminiContent(string Role, IReadOnlyList<GeminiPart> Parts);
    private sealed record GeminiSystemInstruction(IReadOnlyList<GeminiPart> Parts);
    private sealed record GeminiGenerationConfig(int MaxOutputTokens, double Temperature);
    private sealed record GeminiRequest(
        GeminiSystemInstruction SystemInstruction,
        IReadOnlyList<GeminiContent> Contents,
        GeminiGenerationConfig GenerationConfig);
    private sealed record GeminiResponse(IReadOnlyList<GeminiCandidate>? Candidates);
    private sealed record GeminiCandidate(GeminiResponseContent? Content);
    private sealed record GeminiResponseContent(IReadOnlyList<GeminiPart>? Parts);
}
