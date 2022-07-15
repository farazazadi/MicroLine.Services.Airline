using FluentAssertions;
using MicroLine.Services.Airline.Domain.CabinCrew;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.CabinCrew;
public class CabinCrewTests
{
    [Fact]
    public void CabinCrew_ShouldNotHaveAnyEvent_WhenCreated()
    {
        // Given
        // When
        var cabinCrew = Airline.Domain.CabinCrew.CabinCrew.Create(
                            cabinCrewType: CabinCrewType.Purser,
                            gender: Gender.Male,
                            fullName: FullName.Create("Hanna", "Tremblay"),
                            birtDate: Date.Create(1985, 5, 17),
                            nationalId: NationalId.Create("9224150333"),
                            passportNumber: PassportNumber.Create("A43678998"),
                            email: Email.Create("test2@gmail.com"),
                            contactNumber: ContactNumber.Create("+11112223344"),
                            address: Address.Create("129 Novella Rd", "Toronto", "Ontario", "Canada", "589")
                            );

        // Then
        cabinCrew.DomainEvents.Count.Should().Be(0);
    }
}
