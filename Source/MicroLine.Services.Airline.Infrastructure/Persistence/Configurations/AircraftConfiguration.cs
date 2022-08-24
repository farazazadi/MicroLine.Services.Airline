using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Infrastructure.Persistence.Configurations.Extensions;
using MicroLine.Services.Airline.Infrastructure.Persistence.Configurations.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Configurations;

internal class AircraftConfiguration : IEntityTypeConfiguration<Aircraft>
{
    public void Configure(EntityTypeBuilder<Aircraft> builder)
    {

        builder.Property(aircraft => aircraft.Manufacturer)
            .IsRequired();

        builder.Property(aircraft => aircraft.Model)
            .HasConversion(aircraftModel => aircraftModel.ToString(),
                aircraftModelString => AircraftModel.Create(aircraftModelString))
            .HasMaxLength(15)
            .IsRequired();


        builder.Property(aircraft => aircraft.ManufactureDate)
            .IsRequired();

        builder.OwnsOne(aircraft => aircraft.PassengerSeatingCapacity, navigationBuilder =>
        {
            navigationBuilder.Property(passengerSeatingCapacity => passengerSeatingCapacity.EconomyClassCapacity).IsRequired();
            navigationBuilder.Property(passengerSeatingCapacity => passengerSeatingCapacity.BusinessClassCapacity).IsRequired();
            navigationBuilder.Property(passengerSeatingCapacity => passengerSeatingCapacity.FirstClassCapacity).IsRequired();
        })
            .Navigation(aircraft => aircraft.PassengerSeatingCapacity).IsRequired();


        builder.Property(aircraft => aircraft.CruisingSpeed)
            .HasConversion<SpeedConvertor>()
            .IsRequired();


        builder.Property(aircraft => aircraft.MaximumOperatingSpeed)
            .HasConversion<SpeedConvertor>()
            .IsRequired();

        builder.Property(aircraft => aircraft.RegistrationCode)
            .HasConversion(aircraftRegistrationCode => aircraftRegistrationCode.ToString(),
                aircraftRegistrationCodeString => aircraftRegistrationCodeString)
            .HasMaxLength(20)
            .IsRequired();


        builder.AddRowVersion();

    }
}