namespace MicroLine.Services.Airline.Infrastructure.Persistence.DbContextInitializer;

public interface IAirlineDbContextInitializer
{
    Task MigrateAsync(CancellationToken token = default);
    Task SeedAsync(CancellationToken token = default);
}