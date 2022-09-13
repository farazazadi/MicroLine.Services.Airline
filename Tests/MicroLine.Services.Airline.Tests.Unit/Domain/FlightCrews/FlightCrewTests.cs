using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.FlightCrews;

public class FlightCrewTests
{
    [Fact]
    public async Task FlightCrew_ShouldNotHaveAnyEvent_WhenCreated()
    {
        // Given
        var repository = Mock.Of<IFlightCrewReadonlyRepository>();

        // When
        var flightCrew = await FlightCrew.CreateAsync(
                            FlightCrewType.Pilot,
                            Gender.Male,
                            FullName.Create("Gabriel", "Smith"),
                            Date.Create(1991, 2, 10),
                            NationalId.Create("6522255963"),
                            PassportNumber.Create("A43678998"),
                            Email.Create("test@gmail.com"),
                            ContactNumber.Create("+901112223344"),
                            Address.Create("154 Maple Ave", "Toronto", "Ontario", "Canada", "123"),
                            repository
                            );

        // Then
        flightCrew.DomainEvents.Count.Should().Be(0);
    }


    [Fact]
    public async Task FlightCrew_ShouldThrowDuplicatePassportNumberException_WhenPassportNumberAlreadyExist()
    {
        // Given
        var passportNumber = PassportNumber.Create("A43678998");

        var repository = new Mock<IFlightCrewReadonlyRepository>();

        repository
            .Setup(repo => repo.ExistAsync(passportNumber, CancellationToken.None))
            .ReturnsAsync(true);


        // When
        var func = ()=> FlightCrew.CreateAsync(
            FlightCrewType.Pilot,
            Gender.Male,
            FullName.Create("Gabriel", "Smith"),
            Date.Create(1991, 2, 10),
            NationalId.Create("6522255963"),
            passportNumber,
            Email.Create("test@gmail.com"),
            ContactNumber.Create("+901112223344"),
            Address.Create("154 Maple Ave", "Toronto", "Ontario", "Canada", "123"),
            repository.Object
        );


        // Then
        (await func.Should().ThrowExactlyAsync<DuplicatePassportNumberException>())
            .And.Code.Should().Be(nameof(DuplicatePassportNumberException));

    }



    [Fact]
    public async Task FlightCrew_ShouldThrowDuplicateNationalIdException_WhenNationalIdAlreadyExist()
    {
        // Given
        var nationalId = NationalId.Create("6522255963");

        var repository = new Mock<IFlightCrewReadonlyRepository>();

        repository
            .Setup(repo => repo.ExistAsync(nationalId, CancellationToken.None))
            .ReturnsAsync(true);


        // When
        var func = () => FlightCrew.CreateAsync(
            FlightCrewType.Pilot,
            Gender.Male,
            FullName.Create("Gabriel", "Smith"),
            Date.Create(1991, 2, 10),
            nationalId,
            PassportNumber.Create("A43678998"),
            Email.Create("test@gmail.com"),
            ContactNumber.Create("+901112223344"),
            Address.Create("154 Maple Ave", "Toronto", "Ontario", "Canada", "123"),
            repository.Object
        );


        // Then
        (await func.Should().ThrowExactlyAsync<DuplicateNationalIdException>())
            .And.Code.Should().Be(nameof(DuplicateNationalIdException));

    }
}
