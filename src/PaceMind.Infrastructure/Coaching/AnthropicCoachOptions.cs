namespace PaceMind.Infrastructure.Coaching;

public sealed class AnthropicCoachOptions
{
    public string? ApiKey { get; init; }

    public string Model { get; init; } = "claude-opus-4-8";
}
