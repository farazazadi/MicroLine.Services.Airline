using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Tests.Common.Fakes;


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
                                                                48.646900177, -123.426002502);

        // When
        var airport = Airport.Create(
                        icaoCode, iataCode, name, baseUtcOffset, airportLocation);

        // Then
        airport.DomainEvents.Count.Should().Be(0);
    }



    public static TheoryData<double, double, double, double, double> AirportsDistancesData = new()
    {
        //originCoordinate              destinationCoordinate   Distance (Km)
        {49.193901062, -123.183998108,   52.380001, 13.5225,    8004.8408948877777},
        {53.0475006104, 8.78666973114,   53.421299,-6.27007,    1001.0979364076211},
    };

    [Theory, MemberData(nameof(AirportsDistancesData))]
    public void Airport_ShouldCalculateDistanceToAnotherAirport_AsExpected(
        double originAirportLatitude, double originAirportLongitude,
        double destinationAirportLatitude, double destinationAirportLongitude,
        double expectedDistance)
    {
        // Given
        var originAirport = FakeAirport.NewFake(originAirportLatitude, originAirportLongitude);
        var destinationAirport = FakeAirport.NewFake(destinationAirportLatitude, destinationAirportLongitude);

        // When
        var distance = originAirport.GetDistanceTo(destinationAirport, LengthUnit.Kilometer);

        // Then
        distance.Should().Be(expectedDistance);
    }

}
