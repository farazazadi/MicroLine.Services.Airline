using MicroLine.Services.Airline.Domain.Aircraft;
using MicroLine.Services.Airline.Domain.Aircraft.Exceptions;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Aircraft;

public class PassengerSeatingCapacityTests
{

    public static TheoryData<int, int, int> InvalidSeatingCapacityList = new()
    {
        {-1, 10, 20},
        {1, -5, 20},
        {1, 5, -20},
        {0, 0, 0}
    };

    [Theory, MemberData(nameof(InvalidSeatingCapacityList))]
    public void PassengerSeatingCapacity_ShouldThrowInvalidPassengerSeatingCapacityException_WhenItCreatesFromInvalidInput(
        int economyClassCapacity, int businessClassCapacity, int firstClassCapacity)
    {
        // Given
        // When
        var func = () => PassengerSeatingCapacity.Create(economyClassCapacity, businessClassCapacity, firstClassCapacity);

        // Then
        func.Should().ThrowExactly<InvalidPassengerSeatingCapacityException>()
            .And.Code.Should().Be(nameof(InvalidPassengerSeatingCapacityException));
    }


    public static TheoryData<int, int, int> ValidSeatingCapacityList = new()
    {
        {1, 0, 0},
        {0, 1, 0},
        {0, 0, 1},
        {200, 50, 20}
    };

    [Theory, MemberData(nameof(ValidSeatingCapacityList))]
    public void PassengerSeatingCapacity_ShouldHaveExpectedTotalCapacityCreated_WhenItCreatesFromValidInput(
        int economyClassCapacity, int businessClassCapacity, int firstClassCapacity)
    {
        // Given
        var expectedTotalCapacity = economyClassCapacity + businessClassCapacity + firstClassCapacity;
        // When
        var capacity = PassengerSeatingCapacity.Create(economyClassCapacity, businessClassCapacity, firstClassCapacity);

        // Then
        capacity.TotalCapacity.Should().Be(expectedTotalCapacity);
    }

}