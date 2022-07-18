using FluentAssertions;
using MicroLine.Services.Airline.Domain.Airport;
using MicroLine.Services.Airline.Domain.Airport.Exceptions;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Airport;

public class AirportLocationTests
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
    public void AirportLocation_ShouldThrowInvalidAirportLocationException_WhenItCreatesAndCountryIsNullOrEmpty(string country)
    {
        // Given
        // When
        var action = () => AirportLocation.Create(Continent.NorthAmerica, country, "Ontario", "Toronto", 53.309700012200004m, -113.580001831m);

        // Then
        action.Should().ThrowExactly<InvalidAirportLocationException>()
            .And.Code.Should().Be(nameof(InvalidAirportLocationException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void AirportLocation_ShouldThrowInvalidAirportLocationException_WhenItCreatesAndRegionIsNullOrEmpty(string region)
    {
        // Given
        // When
        var action = () => AirportLocation.Create(Continent.NorthAmerica, "Canada", region, "Toronto", 53.309700012200004m, -113.580001831m);

        // Then
        action.Should().ThrowExactly<InvalidAirportLocationException>()
            .And.Code.Should().Be(nameof(InvalidAirportLocationException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void AirportLocation_ShouldThrowInvalidAirportLocationException_WhenItCreatesAndCityIsNullOrEmpty(string city)
    {
        // Given
        // When
        var action = () => AirportLocation.Create(Continent.NorthAmerica, "Canada", "Ontario", city, 53.309700012200004m, -113.580001831m);

        // Then
        action.Should().ThrowExactly<InvalidAirportLocationException>()
            .And.Code.Should().Be(nameof(InvalidAirportLocationException));
    }


    public static TheoryData<decimal> InvalidLatitudeList = new()
    {
        -91.309700012200004m,
        91.309700012200004m,
        91.0000000000m,
        -91.0000000000m
    };

    [Theory, MemberData(nameof(InvalidLatitudeList))]
    public void AirportLocation_ShouldThrowInvalidAirportLocationException_WhenItCreatesAndLatitudeIsInvalid(decimal latitude)
    {
        // Given
        // When
        var action = () => AirportLocation.Create(Continent.NorthAmerica, "Canada", "Ontario", "Toronto", latitude, -113.580001831m);

        // Then
        action.Should().ThrowExactly<InvalidAirportLocationException>()
            .And.Code.Should().Be(nameof(InvalidAirportLocationException));
    }


    public static TheoryData<decimal> InvalidLongitudeList = new()
    {
        -181.580001831m,
        181.580001831m,
        185.000001m,
        -198.012001m,
    };

    [Theory, MemberData(nameof(InvalidLongitudeList))]
    public void AirportLocation_ShouldThrowInvalidAirportLocationException_WhenItCreatesAndLongitudeIsInvalid(decimal longitude)
    {
        // Given
        // When
        var action = () => AirportLocation.Create(Continent.NorthAmerica, "Canada", "Ontario", "Toronto", 53.309700012200004m, longitude);

        // Then
        action.Should().ThrowExactly<InvalidAirportLocationException>()
            .And.Code.Should().Be(nameof(InvalidAirportLocationException));
    }


    [Fact]
    public void AirportLocation_ShouldHaveValidToStringOutput_WhenItCreatedFromValidInput()
    {
        // Given
        var continent = Continent.NorthAmerica;
        var country = "Canada";
        var region = "Ontario";
        var city = "Toronto";
        var latitude = 53.309700012200004m;
        var longitude = -113.580001831m;

        var expected = $"{country}, {region}, {city}, [{latitude}, {longitude}]";

        // When
        var airportLocation = AirportLocation.Create(continent, country, region, city, latitude, longitude);

        // Then
        airportLocation.ToString().Should().Be(expected);

    }

}
