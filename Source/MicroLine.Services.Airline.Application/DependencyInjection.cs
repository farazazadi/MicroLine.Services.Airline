using System.Reflection;
using Mapster;
using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Domain.FlightPricingPolicies;
using Microsoft.Extensions.DependencyInjection;

namespace MicroLine.Services.Airline.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(config => config.RegisterServicesFromAssembly(executingAssembly));

        AddMappers(services, executingAssembly);

        services.AddSingleton<IFlightPricingPolicy, WeekDayFlightPricingPolicy>(
            _ => WeekDayFlightPricingPolicy.Create());

        return services;
    }

    private static IServiceCollection AddMappers(IServiceCollection services, Assembly executingAssembly)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.Default.Settings.MapToConstructor = true;

        config.Scan(executingAssembly);

        services.AddSingleton(config);
        services.AddSingleton<IMapper, Mapper>();

        return services;
    }

}