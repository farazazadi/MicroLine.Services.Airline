using FluentAssertions;
using MicroLine.Services.Airline.Domain.Airport.Exceptions;
using MicroLine.Services.Airline.Domain.Airport;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Airport;
public class AirportNameTests
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
    public void AirportName_ShouldThrowInvalidAirportNameException_WhenItCreatesFromNullOrEmptyString(string airportName)
    {
        // Given
        // When
        var func = () => AirportName.Create(airportName);

        // Then
        func.Should().ThrowExactly<InvalidAirportNameException>()
            .And.Code.Should().Be(nameof(InvalidAirportNameException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void AirportName_ShouldThrowInvalidAirportNameException_WhenItConvertsFromNullOrEmptyString(string airportName)
    {
        // Given
        // When
        var createByImplicitConversationFromString = () => { AirportName name = airportName; };

        // Then
        createByImplicitConversationFromString.Should().ThrowExactly<InvalidAirportNameException>()
            .And.Code.Should().Be(nameof(InvalidAirportNameException));
    }



    public static TheoryData<string> AirportNameStringsWithInvalidLength = new()
    {
        "a",
        "abc             ",
        "           abc",
        "DfqoZq7TfubyboY3V9wwDuTV15Jvj6KV7ybQy1grargD0ziPg34ekVXzVEJwz",
        "DfqoZq7TfubyboY3V9ww DuTV15Jvj6KV7ybQy1gr argD0ziPg34ekVXzVEJw",
    };

    [Theory, MemberData(nameof(AirportNameStringsWithInvalidLength))]
    public void AirportName_ShouldThrowInvalidAirportNameException_WhenItCreatesFromStringWithLengthLessThan4OrGreaterThan60(string airportName)
    {
        // Given
        // When
        var func = () => AirportName.Create(airportName);

        // Then
        func.Should().ThrowExactly<InvalidAirportNameException>()
            .And.Code.Should().Be(nameof(InvalidAirportNameException));
    }


    public static TheoryData<string> ValidAirportNameStrings = new()
    {
        "Edmonton International Airport",
        "Sydney Kingsford Smith International Airportt",
        "ENBO"
    };
    [Theory, MemberData(nameof(ValidAirportNameStrings))]
    public void AirportName_ShouldCreate_WhenItCreatesFromValidString(string airportName)
    {
        // Given
        // When
        var name = AirportName.Create(airportName);

        // Then
        name.ToString().Should().Be(airportName.Trim());
    }
}