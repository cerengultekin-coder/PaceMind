using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaceMind.Application.Coaching;
using PaceMind.Infrastructure.Coaching;

namespace PaceMind.Infrastructure;

/// <summary>Composition root for infrastructure services (external providers).</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the AI coach. The API key is read from <c>Anthropic:ApiKey</c> or the
    /// <c>ANTHROPIC_API_KEY</c> environment variable; the model from <c>Anthropic:Model</c>
    /// (default <c>claude-opus-4-8</c>). Absent a key, the coach returns friendly placeholders.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new AnthropicCoachOptions
        {
            ApiKey = configuration["Anthropic:ApiKey"] ?? Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY"),
            Model = configuration["Anthropic:Model"] ?? "claude-opus-4-8",
        };

        services.AddSingleton(options);
        services.AddSingleton<ICoachService, AnthropicCoachService>();
        return services;
    }
}
