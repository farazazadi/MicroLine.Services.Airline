using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Infrastructure.Persistence.Configurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Configurations;

internal class CabinCrewConfiguration : IEntityTypeConfiguration<CabinCrew>
{
    public void Configure(EntityTypeBuilder<CabinCrew> builder)
    {
        builder.Property(cabinCrew => cabinCrew.CabinCrewType)
            .IsRequired();

        builder.Property(cabinCrew => cabinCrew.Gender)
            .IsRequired();

        builder.OwnsOne(cabinCrew => cabinCrew.FullName, navigationBuilder =>
        {
            navigationBuilder.Property(fullName => fullName.FirstName).HasMaxLength(50).IsRequired();
            navigationBuilder.Property(fullName => fullName.LastName).HasMaxLength(50).IsRequired();
        })
            .Navigation(cabinCrew => cabinCrew.FullName).IsRequired();

        builder.Property(cabinCrew => cabinCrew.BirthDate)
            .IsRequired();

        builder.Property(cabinCrew => cabinCrew.NationalId)
            .HasConversion(nationalId => nationalId.ToString(),
                nationalIdString => nationalIdString)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(cabinCrew => cabinCrew.PassportNumber)
            .HasConversion(passportNumber => passportNumber.ToString(),
                passportNumberString => passportNumberString)
            .HasMaxLength(9)
            .IsRequired();


        builder.Property(cabinCrew => cabinCrew.Email)
            .HasConversion(email => email.ToString(),
                emailString => emailString)
            .HasMaxLength(100)
            .IsRequired();


        builder.Property(cabinCrew => cabinCrew.ContactNumber)
            .HasConversion(contactNumber => contactNumber.ToString(),
                contactNumberString => contactNumberString)
            .HasMaxLength(15)
            .IsRequired();


        builder.OwnsOne(cabinCrew => cabinCrew.Address, navigationBuilder =>
        {
            navigationBuilder.Property(address => address.Street).HasMaxLength(50).IsRequired();
            navigationBuilder.Property(address => address.City).HasMaxLength(50).IsRequired();
            navigationBuilder.Property(address => address.State).HasMaxLength(50).IsRequired();
            navigationBuilder.Property(address => address.Country).HasMaxLength(50).IsRequired();
            navigationBuilder.Property(address => address.PostalCode).HasMaxLength(50).IsRequired();
        })
            .Navigation(cabinCrew => cabinCrew.Address).IsRequired();


        builder.AddRowVersion();
    }
}