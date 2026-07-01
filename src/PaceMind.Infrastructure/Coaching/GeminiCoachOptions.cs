namespace PaceMind.Infrastructure.Coaching;

/// <summary>Settings for the Google Gemini coach (free tier). The API key stays server-side.</summary>
public sealed class GeminiCoachOptions
{
    public string? ApiKey { get; init; }

    public string Model { get; init; } = "gemini-2.5-flash";
}
