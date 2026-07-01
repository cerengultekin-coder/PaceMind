namespace PaceMind.Infrastructure.Coaching;

/// <summary>Provider settings for the AI coach. The API key stays server-side.</summary>
public sealed class AnthropicCoachOptions
{
    public string? ApiKey { get; init; }

    public string Model { get; init; } = "claude-opus-4-8";
}
