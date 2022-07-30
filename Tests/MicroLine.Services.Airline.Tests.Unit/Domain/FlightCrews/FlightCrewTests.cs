using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.FlightCrews;

public class FlightCrewTests
{
    [Fact]
    public void FlightCrew_ShouldNotHaveAnyEvent_WhenCreated()
    {
        // Given
        // When
        var flightCrew = FlightCrew.Create(
                            flightCrewType: FlightCrewType.Pilot,
                            gender: Gender.Male,
                            fullName: FullName.Create("Gabriel", "Smith"),
                            birthDate: Date.Create(1991, 2, 10),
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
