using MicroLine.Services.Airline.Application;
using MicroLine.Services.Airline.Infrastructure;
using MicroLine.Services.Airline.WebApi;
using MicroLine.Services.Airline.Infrastructure.Persistence.DbContextInitializer;
using MicroLine.Services.Airline.WebApi.Airports;
using MicroLine.Services.Airline.WebApi.Common.Middleware;
using Serilog;

const string serviceName = "Airline";

Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("Service", serviceName)
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting {Service} service up...", serviceName);

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) => loggerConfiguration
        .Enrich.WithProperty("Service", serviceName)
        .ReadFrom.Configuration(hostBuilderContext.Configuration)
    );

    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddWebApi();

    var app = builder.Build();


    if (app.Environment.IsDevelopment())
    {
        app.UseSerilogRequestLogging();

        app.UseSwagger();
        app.UseSwaggerUI();

        using (var scope = app.Services.CreateScope())
        {
            var dbContextInitializer = scope.ServiceProvider.GetRequiredService<IAirlineDbContextInitializer>();

            await dbContextInitializer.MigrateAsync();
            await dbContextInitializer.SeedAsync();
        }
    }

    app
        .UseMiddleware<ExceptionHandlingMiddleware>()
        .UseHttpsRedirection();

    app.MapGet("/", () => "MicroLine.Services.Airline");

    app.MapAirportEndpoints();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "The {Service} service could not be started!", serviceName);
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program{}