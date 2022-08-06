using MicroLine.Services.Airline.Domain.Flights;
using MicroLine.Services.Airline.Domain.Flights.Exceptions;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Flights;

public class FlightNumberTests
{
    public static TheoryData<string> NullOrEmptyStrings = new()
    {
        "",
        " ",
        "        ",
        string.Empty,
        null
    };

    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void FlightNumber_ShouldThrowInvalidFlightNumberException_WhenItCreatesFromNullOrEmptyString(string flightNumber)
    {
        // Given
        // When
        var func = () => FlightNumber.Create(flightNumber);

        // Then
        func.Should().ThrowExactly<InvalidFlightNumberException>()
            .And.Code.Should().Be(nameof(InvalidFlightNumberException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void FlightNumber_ShouldThrowInvalidFlightNumberException_WhenItConvertsFromNullOrEmptyString(string flightNumber)
    {
        // Given
        // When
        var action = () => { FlightNumber number = flightNumber; };

        // Then
        action.Should().ThrowExactly<InvalidFlightNumberException>()
            .And.Code.Should().Be(nameof(InvalidFlightNumberException));
    }



    public static TheoryData<string> FlightNumberStringsWithInvalidLength = new()
    {
        "a5",
        "ab5             ",
        "           ab5",
        "ab1234567"
    };

    [Theory, MemberData(nameof(FlightNumberStringsWithInvalidLength))]
    public void FlightNumber_ShouldThrowInvalidFlightNumberException_WhenItCreatesFromStringWithLengthLessThan4OrGreaterThan8(string flightNumber)
    {
        // Given
        // When
        var func = () => FlightNumber.Create(flightNumber);

        // Then
        func.Should().ThrowExactly<InvalidFlightNumberException>()
            .And.Code.Should().Be(nameof(InvalidFlightNumberException));
    }



    public static TheoryData<string> InvalidFlightNumberStrings = new()
    {
        "c680h",
        "GLF5A",
        "077AFR",
        "123",
        "1234567"
    };

    [Theory, MemberData(nameof(InvalidFlightNumberStrings))]
    public void FlightNumber_ShouldThrowInvalidFlightNumberException_WhenItCreatesFromInvalidString(string flightNumber)
    {
        // Given
        // When
        var func = () => FlightNumber.Create(flightNumber);

        // Then
        func.Should().ThrowExactly<InvalidFlightNumberException>()
            .And.Code.Should().Be(nameof(InvalidFlightNumberException));
    }



    public static TheoryData<string> ValidFlightNumberStrings = new()
    {
        "c680",
        "GLF5",
        "AFR077",
        "FTH399",
        "SWA2605"
    };
    [Theory, MemberData(nameof(ValidFlightNumberStrings))]
    public void FlightNumber_ShouldHaveUpperCaseToStringOutPut_WhenItCreatesFromValidString(string flightNumber)
    {
        // Given
        // When
        var number = FlightNumber.Create(flightNumber);

        // Then
        number.ToString().Should().Be(flightNumber.Trim().ToUpperInvariant());
    }
}
