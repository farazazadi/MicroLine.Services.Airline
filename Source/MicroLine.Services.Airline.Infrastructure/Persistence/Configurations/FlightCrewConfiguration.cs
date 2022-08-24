using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Infrastructure.Persistence.Configurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Configurations;

internal class FlightCrewConfiguration : IEntityTypeConfiguration<FlightCrew>
{
    public void Configure(EntityTypeBuilder<FlightCrew> builder)
    {
        builder.Property(flightCrew => flightCrew.FlightCrewType)
            .IsRequired();

        builder.Property(flightCrew => flightCrew.Gender)
            .IsRequired();

        builder.OwnsOne(flightCrew => flightCrew.FullName, navigationBuilder =>
        {
            navigationBuilder.Property(fullName => fullName.FirstName).HasMaxLength(50).IsRequired();
            navigationBuilder.Property(fullName => fullName.LastName).HasMaxLength(50).IsRequired();
        })
            .Navigation(flightCrew => flightCrew.FullName).IsRequired();

        builder.Property(flightCrew => flightCrew.BirthDate)
            .IsRequired();

        builder.Property(flightCrew => flightCrew.NationalId)
            .HasConversion(nationalId => nationalId.ToString(),
                nationalIdString => nationalIdString)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(flightCrew => flightCrew.PassportNumber)
            .HasConversion(passportNumber => passportNumber.ToString(),
                passportNumberString => passportNumberString)
            .HasMaxLength(9)
            .IsRequired();


        builder.Property(flightCrew => flightCrew.Email)
            .HasConversion(email => email.ToString(),
                emailString => emailString)
            .HasMaxLength(100)
            .IsRequired();


        builder.Property(flightCrew => flightCrew.ContactNumber)
            .HasConversion(contactNumber => contactNumber.ToString(),
                contactNumberString => contactNumberString)
            .HasMaxLength(15)
            .IsRequired();


        builder.OwnsOne(flightCrew => flightCrew.Address, navigationBuilder =>
        {
            navigationBuilder.Property(address => address.Street).HasMaxLength(50).IsRequired();
            navigationBuilder.Property(address => address.City).HasMaxLength(50).IsRequired();
            navigationBuilder.Property(address => address.State).HasMaxLength(50).IsRequired();
            navigationBuilder.Property(address => address.Country).HasMaxLength(50).IsRequired();
            navigationBuilder.Property(address => address.PostalCode).HasMaxLength(50).IsRequired();
        })
            .Navigation(flightCrew => flightCrew.Address).IsRequired();


        builder.AddRowVersion();
    }
}