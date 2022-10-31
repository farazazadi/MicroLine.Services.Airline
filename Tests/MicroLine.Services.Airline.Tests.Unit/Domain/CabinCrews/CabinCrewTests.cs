using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.CabinCrews;
public class CabinCrewTests
{
    [Fact]
    public async Task CabinCrew_ShouldNotHaveAnyEvent_WhenCreated()
    {
        // Given
        var repository = Mock.Of<ICabinCrewReadonlyRepository>();

        // When
        var cabinCrew = await CabinCrew.CreateAsync(
                            CabinCrewType.Purser,
                            Gender.Female,
                            FullName.Create("Hanna", "Tremblay"),
                            Date.Create(1985, 5, 17),
                            NationalId.Create("9224150333"),
                            PassportNumber.Create("A43678998"),
                            Email.Create("test2@gmail.com"),
                            ContactNumber.Create("+11112223344"),
                            Address.Create("129 Novella Rd", "Toronto", "Ontario", "Canada", "589"),
                            repository
                            );

        // Then
        cabinCrew.DomainEvents.Count.Should().Be(0);
    }

    [Fact]
    public async Task CabinCrew_ShouldThrowDuplicatePassportNumberException_WhenPassportNumberAlreadyExist()
    {
        // Given
        var passportNumber = PassportNumber.Create("A43671942");

        var repository = new Mock<ICabinCrewReadonlyRepository>();

        repository
            .Setup(repo => repo.ExistAsync(passportNumber, CancellationToken.None))
            .ReturnsAsync(true);


        // When
        var func = () => CabinCrew.CreateAsync(
            CabinCrewType.Purser,
            Gender.Female,
            FullName.Create("Hanna", "Tremblay"),
            Date.Create(1985, 5, 17),
            NationalId.Create("9224150333"),
            passportNumber,
            Email.Create("test2@gmail.com"),
            ContactNumber.Create("+11112223344"),
            Address.Create("129 Novella Rd", "Toronto", "Ontario", "Canada", "589"),
            repository.Object
        );


        // Then
        (await func.Should().ThrowExactlyAsync<DuplicatePassportNumberException>())
            .And.Code.Should().Be(nameof(DuplicatePassportNumberException));

    }
}
