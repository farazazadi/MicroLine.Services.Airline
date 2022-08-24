using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Infrastructure.Persistence.Configurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Configurations;

internal class AirportConfiguration : IEntityTypeConfiguration<Airport>
{
    public void Configure(EntityTypeBuilder<Airport> builder)
    {
        builder.Property(airport => airport.IcaoCode)
            .HasConversion(icaoCode => icaoCode.ToString(), icaoCodeString => icaoCodeString)
            .HasMaxLength(4)
            .IsRequired();

        builder.Property(airport => airport.IataCode)
            .HasConversion(iataCode => iataCode.ToString(), iataCodeString => iataCodeString)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(airport => airport.Name)
            .HasConversion(airportName => airportName.ToString(), nameString => nameString)
            .HasMaxLength(60)
            .IsRequired();

        builder.OwnsOne(airport => airport.BaseUtcOffset, navigationBuilder =>
        {
            navigationBuilder.Property(baseUtcOffset => baseUtcOffset.Hours).IsRequired();
            navigationBuilder.Property(baseUtcOffset => baseUtcOffset.Minutes).IsRequired();
        })
            .Navigation(airport => airport.BaseUtcOffset).IsRequired();

        builder.OwnsOne(airport => airport.AirportLocation, navigationBuilder =>
        {
            navigationBuilder.Property(airportLocation => airportLocation.Continent).IsRequired();
            navigationBuilder.Property(airportLocation => airportLocation.Country).IsRequired();
            navigationBuilder.Property(airportLocation => airportLocation.Region).IsRequired();
            navigationBuilder.Property(airportLocation => airportLocation.City).IsRequired();

            navigationBuilder.Property(airportLocation => airportLocation.Latitude).IsRequired();
            navigationBuilder.Property(airportLocation => airportLocation.Longitude).IsRequired();
        })
            .Navigation(airport => airport.AirportLocation).IsRequired();


        builder.AddRowVersion();

    }
}