using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MicroLine.Services.Airline.Infrastructure.Persistence.Options;
using Respawn;
using Respawn.Graph;
using MicroLine.Services.Airline.Infrastructure.Persistence.DbContextInitializer;

namespace MicroLine.Services.Airline.Tests.Integration.Common;

public sealed class AirlineWebApplicationFactory : WebApplicationFactory<Program>
{
    private DatabaseOptions _databaseOptions;

    private readonly Checkpoint _checkpoint = new()
    {
        TablesToIgnore = new Table[] { "__EFMigrationsHistory" }
    };

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();

            _databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

            var dbContextInitializer = serviceProvider.GetRequiredService<IAirlineDbContextInitializer>();
            Task.Run(async () => await dbContextInitializer.MigrateAsync());
        });

        base.ConfigureWebHost(builder);
    }


    public async Task ResetDatabaseAsync()
    {
        await _checkpoint.Reset(_databaseOptions.ConnectionString);
    }

    public override async ValueTask DisposeAsync()
    {
        await ResetDatabaseAsync();

        await base.DisposeAsync();
    }
}