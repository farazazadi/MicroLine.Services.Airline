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
        services.AddMediatR(Assembly.GetExecutingAssembly());

        AddMappers(services);

        services.AddSingleton<IFlightPricingPolicy, WeekDayFlightPricingPolicy>(
            _ => WeekDayFlightPricingPolicy.Create());

        return services;
    }

    private static IServiceCollection AddMappers(IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.Default.Settings.MapToConstructor = true;

        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

}