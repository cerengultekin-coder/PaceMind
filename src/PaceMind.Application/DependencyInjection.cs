using Microsoft.Extensions.DependencyInjection;
using PaceMind.Domain.Training;
using PaceMind.Domain.Training.Adaptation;
using PaceMind.Domain.Training.Planning;
using PaceMind.Domain.Training.Profiles;

namespace PaceMind.Application;

/// <summary>Composition root for the application's training engine.</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the sport profiles, profile resolver, and plan generator. Profiles and the
    /// resolver are stateless, so they are registered as singletons.
    /// </summary>
    public static IServiceCollection AddTrainingEngine(this IServiceCollection services)
    {
        services.AddSingleton<ISportProfile, RunningProfile>();
        services.AddSingleton<ISportProfileResolver, SportProfileResolver>();
        services.AddSingleton<IPlanGenerator, PlanGenerator>();
        services.AddSingleton<IWeekAdapter, WeekAdapter>();
        return services;
    }
}
