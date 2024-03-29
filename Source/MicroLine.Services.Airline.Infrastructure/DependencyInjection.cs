using System.Reflection;
using Mapster;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Infrastructure.Persistence;
using MicroLine.Services.Airline.Infrastructure.Persistence.DbContextInitializer;
using MicroLine.Services.Airline.Infrastructure.Persistence.Interceptors;
using MicroLine.Services.Airline.Infrastructure.Persistence.Options;
using MicroLine.Services.Airline.Infrastructure.Services;
using MicroLine.Services.Airline.Infrastructure.Services.Outbox;
using MicroLine.Services.Airline.Infrastructure.Services.RabbitMq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MicroLine.Services.Airline.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);

        services
            .AddScoped<IEventDispatcher, EventDispatcherService>()
            .AddTransient<IDateTime, DateTimeService>();

        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.AddSingleton<RabbitMqPublisher>();

        services.Configure<OutboxProcessorOptions>(configuration.GetSection(OutboxProcessorOptions.SectionName));
        services.AddHostedService<OutboxProcessor>();

        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

        return services;
    }


    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var databaseOptions = new DatabaseOptions(configuration);
        configuration.Bind(databaseOptions.SectionName, databaseOptions);

        services.AddSingleton(Options.Create(databaseOptions));

        services.AddDbContext<AirlineDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(databaseOptions.ConnectionString,
                sqlServerDbContextOptionsBuilder =>
                {
                    sqlServerDbContextOptionsBuilder.MigrationsAssembly(typeof(AirlineDbContext).Assembly.FullName);
                    sqlServerDbContextOptionsBuilder.CommandTimeout(databaseOptions.CommandTimeout);
                    sqlServerDbContextOptionsBuilder.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
                });

            optionsBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
            optionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
        });

        services
            .AddScoped<IAirlineDbContext>(provider => provider.GetRequiredService<AirlineDbContext>())
            .AddScoped<DbContext>(provider => provider.GetRequiredService<AirlineDbContext>())
            .AddScoped<IAirlineDbContextInitializer, AirlineDbContextInitializer>()
            .AddScoped<ISaveChangesInterceptor, AuditingInterceptor>()
            .AddScoped<ISaveChangesInterceptor, EventDispatchingInterceptor>()
            .AddScoped<ISaveChangesInterceptor, OutboxInterceptor>();
    }
}