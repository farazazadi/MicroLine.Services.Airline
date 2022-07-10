using FluentAssertions;
using MicroLine.Services.Airline.Domain.Aircraft;
using MicroLine.Services.Airline.Domain.Aircraft.Exceptions;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Aircraft;

public class AircraftMaximumSeatingCapacityTests
{

    public static TheoryData<int> SeatingCapacitiesLessThan2 = new()
    {
        -1,
        -2,
        -25000,
        0,
        1,
    };

    [Theory, MemberData(nameof(SeatingCapacitiesLessThan2))]
    public void AircraftMaximumSeatingCapacity_ShouldThrowInvalidAircraftMaximumSeatingCapacityException_WhenItCreatesFromInputLessThan2(int aircraftMaximumSeatingCapacity)
    {
        // Given
        // When
        var func = () => AircraftMaximumSeatingCapacity.Create(aircraftMaximumSeatingCapacity);

        // Then
        func.Should().ThrowExactly<InvalidAircraftMaximumSeatingCapacityException>()
            .And.Code.Should().Be(nameof(InvalidAircraftMaximumSeatingCapacityException));
    }

    [Theory, MemberData(nameof(SeatingCapacitiesLessThan2))]
    public void AircraftMaximumSeatingCapacity_ShouldThrowInvalidAircraftMaximumSeatingCapacityException_WhenItConvertsFromInputLessThan2(int aircraftMaximumSeatingCapacity)
    {
        // Given
        // When
        var action = () => { AircraftMaximumSeatingCapacity capacity = aircraftMaximumSeatingCapacity; };
    
        // Then
        action.Should().ThrowExactly<InvalidAircraftMaximumSeatingCapacityException>()
            .And.Code.Should().Be(nameof(InvalidAircraftMaximumSeatingCapacityException));
    }

    

    public static TheoryData<int> SeatingCapacitiesGreaterThan2 = new()
    {
        3,
        5,
        500,
        120,
        280
    };
    
    [Theory, MemberData(nameof(SeatingCapacitiesGreaterThan2))]
    public void AircraftMaximumSeatingCapacity_ShouldBeCreated_WhenItCreatesFromValidInput(int aircraftMaximumSeatingCapacity)
    {
        // Given
        // When
        var capacity = AircraftMaximumSeatingCapacity.Create(aircraftMaximumSeatingCapacity);

        // Then
        aircraftMaximumSeatingCapacity.Should().Be(capacity);
    }
    
    [Theory, MemberData(nameof(SeatingCapacitiesGreaterThan2))]
    public void AircraftMaximumSeatingCapacity_ShouldBeCreated_WhenItConvertsFromValidInput(int aircraftMaximumSeatingCapacity)
    {
        // Given
        // When
        var capacity = (AircraftMaximumSeatingCapacity)aircraftMaximumSeatingCapacity;

        // Then
        aircraftMaximumSeatingCapacity.Should().Be(capacity);
    }
}

