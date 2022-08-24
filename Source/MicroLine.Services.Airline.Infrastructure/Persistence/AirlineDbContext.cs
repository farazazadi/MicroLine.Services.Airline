using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Domain.Flights;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MicroLine.Services.Airline.Infrastructure.Persistence.Configurations.ValueConverters;

namespace MicroLine.Services.Airline.Infrastructure.Persistence;

internal sealed class AirlineDbContext : DbContext, IAirlineDbContext
{
    public DbSet<Airport> Airports { get; init; }
    public DbSet<Aircraft> Aircrafts { get; init; }
    public DbSet<CabinCrew> CabinCrews { get; init; }
    public DbSet<FlightCrew> FlightCrews { get; init; }
    public DbSet<Flight> Flights { get; init; }

    private readonly IEnumerable<ISaveChangesInterceptor> _saveChangesInterceptors;

    public AirlineDbContext(DbContextOptions<AirlineDbContext> options, IEnumerable<ISaveChangesInterceptor> saveChangesInterceptors)
        : base(options)
    {
        _saveChangesInterceptors = saveChangesInterceptors;

    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_saveChangesInterceptors);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.IgnoreAny<IReadOnlyList<DomainEvent>>();

        configurationBuilder.Properties<Id>()
            .HaveConversion<IdConvertor>();

        configurationBuilder.Properties<Date>()
            .HaveColumnType("date")
            .HaveConversion<DateConvertor>();


        base.ConfigureConventions(configurationBuilder);
    }
}