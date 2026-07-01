using Microsoft.Extensions.DependencyInjection;
using PaceMind.Domain.Training;
using PaceMind.Domain.Training.Adaptation;
using PaceMind.Domain.Training.Planning;
using PaceMind.Domain.Training.Profiles;

namespace PaceMind.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddTrainingEngine(this IServiceCollection services)
    {
        services.AddSingleton<ISportProfile, RunningProfile>();
        services.AddSingleton<ISportProfileResolver, SportProfileResolver>();
        services.AddSingleton<IPlanGenerator, PlanGenerator>();
        services.AddSingleton<IWeekAdapter, WeekAdapter>();
        return services;
    }
}
