namespace PaceMind.Infrastructure.Coaching;

public sealed class GeminiCoachOptions
{
    public string? ApiKey { get; init; }

    public string Model { get; init; } = "gemini-2.5-flash";
}
