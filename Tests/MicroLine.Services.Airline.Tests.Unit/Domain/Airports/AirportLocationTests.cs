using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.Airports.Exceptions;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Airports;

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
        var action = () => AirportLocation.Create(Continent.NorthAmerica, country, "Ontario", "Toronto", 53.309700012200004, -113.580001831);

        // Then
        action.Should().ThrowExactly<InvalidAirportLocationException>()
            .And.Code.Should().Be(nameof(InvalidAirportLocationException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void AirportLocation_ShouldThrowInvalidAirportLocationException_WhenItCreatesAndRegionIsNullOrEmpty(string region)
    {
        // Given
        // When
        var action = () => AirportLocation.Create(Continent.NorthAmerica, "Canada", region, "Toronto", 53.309700012200004, -113.580001831);

        // Then
        action.Should().ThrowExactly<InvalidAirportLocationException>()
            .And.Code.Should().Be(nameof(InvalidAirportLocationException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void AirportLocation_ShouldThrowInvalidAirportLocationException_WhenItCreatesAndCityIsNullOrEmpty(string city)
    {
        // Given
        // When
        var action = () => AirportLocation.Create(Continent.NorthAmerica, "Canada", "Ontario", city, 53.309700012200004, -113.580001831);

        // Then
        action.Should().ThrowExactly<InvalidAirportLocationException>()
            .And.Code.Should().Be(nameof(InvalidAirportLocationException));
    }


    public static TheoryData<double> InvalidLatitudeList = new()
    {
        -91.309700012200004,
        91.309700012200004,
        91.0000000000,
        -91.0000000000
    };

    [Theory, MemberData(nameof(InvalidLatitudeList))]
    public void AirportLocation_ShouldThrowInvalidAirportLocationException_WhenItCreatesAndLatitudeIsInvalid(double latitude)
    {
        // Given
        // When
        var action = () => AirportLocation.Create(Continent.NorthAmerica, "Canada", "Ontario", "Toronto", latitude, -113.580001831);

        // Then
        action.Should().ThrowExactly<InvalidAirportLocationException>()
            .And.Code.Should().Be(nameof(InvalidAirportLocationException));
    }


    public static TheoryData<double> InvalidLongitudeList = new()
    {
        -181.580001831,
        181.580001831,
        185.000001,
        -198.012001,
    };

    [Theory, MemberData(nameof(InvalidLongitudeList))]
    public void AirportLocation_ShouldThrowInvalidAirportLocationException_WhenItCreatesAndLongitudeIsInvalid(double longitude)
    {
        // Given
        // When
        var action = () => AirportLocation.Create(Continent.NorthAmerica, "Canada", "Ontario", "Toronto", 53.309700012200004, longitude);

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
        var latitude = 53.309700012200004;
        var longitude = -113.580001831;

        var expected = $"{country}, {region}, {city}, [{latitude}, {longitude}]";

        // When
        var airportLocation = AirportLocation.Create(continent, country, region, city, latitude, longitude);

        // Then
        airportLocation.ToString().Should().Be(expected);

    }

}
