using FluentAssertions;
using MicroLine.Services.Airline.Domain.Airport;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Airport;

public class AirportTests
{
    [Fact]
    public void Airport_ShouldNotHavAnyEvents_WhenCreated()
    {
        // Given
        var icaoCode = IcaoCode.Create("CYYJ");
        var iataCode = IataCode.Create("YYJ");
        var name = AirportName.Create("Victoria International Airport");
        var airportLocation = AirportLocation.Create(Continent.NorthAmerica, "Canada", "British Columbia", "Victoria",
                                                                48.646900177m, -123.426002502m);

        // When
        var airport = Airline.Domain.Airport.Airport.Create(
                    icaoCode: icaoCode,
                    iataCode: iataCode,
                    name: name,
                    airportLocation: airportLocation
                );

        // Then
        airport.DomainEvents.Count.Should().Be(0);
    }
}
