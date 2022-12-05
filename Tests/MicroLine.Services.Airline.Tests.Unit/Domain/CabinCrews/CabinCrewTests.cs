using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.CabinCrews;
public class CabinCrewTests
{
    [Fact]
    public void CabinCrew_ShouldNotHaveAnyEvent_WhenCreated()
    {
        // Given

        // When
        var cabinCrew = CabinCrew.Create(
                            CabinCrewType.Purser,
                            Gender.Female,
                            FullName.Create("Hanna", "Tremblay"),
                            Date.Create(1985, 5, 17),
                            NationalId.Create("9224150333"),
                            PassportNumber.Create("A43678998"),
                            Email.Create("test2@gmail.com"),
                            ContactNumber.Create("+11112223344"),
                            Address.Create("129 Novella Rd", "Toronto", "Ontario", "Canada", "589")
                            );

        // Then
        cabinCrew.DomainEvents.Count.Should().Be(0);
    }

}
