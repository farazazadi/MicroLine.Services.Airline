using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.WebApi.Services;

namespace MicroLine.Services.Airline.WebApi;

internal static class DependencyInjection
{
    public static IServiceCollection AddWebApi(this IServiceCollection services)
    {
        services
            .AddSwaggerGen()
            .AddEndpointsApiExplorer()
            .AddHttpContextAccessor();

        services.AddSingleton<ICurrentUser, CurrentUserService>();

        return services;
    }
}