using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Airports;

public class AirportTests
{
    [Fact]
    public void Airport_ShouldNotHavAnyEvents_WhenCreated()
    {
        // Given
        var icaoCode = IcaoCode.Create("CYYJ");
        var iataCode = IataCode.Create("YYJ");
        var name = AirportName.Create("Victoria International Airport");
        var baseUtcOffset = BaseUtcOffset.Create(-7, 0);
        var airportLocation = AirportLocation.Create(Continent.NorthAmerica, "Canada", "British Columbia", "Victoria",
                                                                48.646900177m, -123.426002502m);

        // When
        var airport = Airport.Create(
                        icaoCode, iataCode, name, baseUtcOffset, airportLocation);

        // Then
        airport.DomainEvents.Count.Should().Be(0);
    }
}
