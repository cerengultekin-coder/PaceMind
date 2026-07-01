using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaceMind.Application.Coaching;
using PaceMind.Infrastructure.Coaching;

namespace PaceMind.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration["Coach:Provider"];

        if (string.Equals(provider, "Anthropic", StringComparison.OrdinalIgnoreCase))
        {
            var anthropic = new AnthropicCoachOptions
            {
                ApiKey = configuration["Anthropic:ApiKey"] ?? Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY"),
                Model = configuration["Anthropic:Model"] ?? "claude-opus-4-8",
            };
            services.AddSingleton(anthropic);
            services.AddSingleton<ICoachService, AnthropicCoachService>();
        }
        else
        {
            var gemini = new GeminiCoachOptions
            {
                ApiKey = configuration["Gemini:ApiKey"] ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY"),
                Model = configuration["Gemini:Model"] ?? "gemini-2.5-flash",
            };
            services.AddSingleton(gemini);
            services.AddSingleton<ICoachService, GeminiCoachService>();
        }

        return services;
    }
}
