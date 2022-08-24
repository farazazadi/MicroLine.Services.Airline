using MicroLine.Services.Airline.Domain.Flights;
using MicroLine.Services.Airline.Infrastructure.Persistence.Configurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Configurations;

internal class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.Property(flight => flight.FlightNumber)
            .HasConversion(flightNumber => flightNumber.ToString(),
                flightNumberString => flightNumberString)
            .HasMaxLength(8)
            .IsRequired();

        builder.HasOne(flight => flight.OriginAirport)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(flight => flight.DestinationAirport)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(flight => flight.Aircraft)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.Property(flight => flight.ScheduledUtcDateTimeOfDeparture)
            .IsRequired();

        builder.Property(flight => flight.ScheduledUtcDateTimeOfDeparture)
            .IsRequired();

        builder.OwnsOne(flight => flight.Prices, navigationBuilder =>
        {
            navigationBuilder.OwnsOne(flightPrice => flightPrice.EconomyClass, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.Property(money => money.Amount)
                    .HasPrecision(11, 6)
                    .IsRequired();

                ownedNavigationBuilder.Property(money => money.Currency).IsRequired();
            })
                .Navigation(flightPrice=> flightPrice.EconomyClass).IsRequired();


            navigationBuilder.OwnsOne(flightPrice => flightPrice.BusinessClass, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.Property(money => money.Amount)
                    .HasPrecision(11, 6)
                    .IsRequired();

                ownedNavigationBuilder.Property(money => money.Currency).IsRequired();
            })
                .Navigation(flightPrice => flightPrice.BusinessClass).IsRequired();


            navigationBuilder.OwnsOne(flightPrice => flightPrice.FirstClass, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.Property(money => money.Amount)
                    .HasPrecision(11, 6)
                    .IsRequired();

                ownedNavigationBuilder.Property(money => money.Currency).IsRequired();
            })
                .Navigation(flightPrice => flightPrice.FirstClass).IsRequired();

        })
            .Navigation(a=> a.Prices).IsRequired();


        builder.HasMany(flight => flight.FlightCrewMembers)
            .WithMany(flightCrew => flightCrew.Flights);

        builder.HasMany(flight => flight.CabinCrewMembers)
            .WithMany(cabinCrew => cabinCrew.Flights);

        builder.Property(f => f.Status)
            .IsRequired();


        builder.AddRowVersion();

    }
}