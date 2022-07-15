using FluentAssertions;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrew;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.FlightCrew;

public class FlightCrewTests
{
    [Fact]
    public void FlightCrew_ShouldNotHaveAnyEvent_WhenCreated()
    {
        // Given
        // When
        var flightCrew = Airline.Domain.FlightCrew.FlightCrew.Create(
                            flightCrewType: FlightCrewType.Pilot,
                            gender: Gender.Male,
                            fullName: FullName.Create("Gabriel", "Smith"),
                            birtDate: Date.Create(1991, 2, 10),
                            nationalId: NationalId.Create("6522255963"),
                            passportNumber: PassportNumber.Create("A43678998"),
                            email: Email.Create("test@gmail.com"),
                            contactNumber: ContactNumber.Create("+901112223344"),
                            address: Address.Create("154 Maple Ave", "Toronto", "Ontario", "Canada", "123")
                            );

        // Then
        flightCrew.DomainEvents.Count.Should().Be(0);
    }

}
