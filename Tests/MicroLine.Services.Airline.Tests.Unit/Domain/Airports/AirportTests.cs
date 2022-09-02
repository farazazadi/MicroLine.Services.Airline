using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.Airports.Exceptions;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Tests.Common.Fakes;


namespace MicroLine.Services.Airline.Tests.Unit.Domain.Airports;

public class AirportTests
{
    [Fact]
    public async Task Airport_ShouldNotHavAnyEvents_WhenCreated()
    {
        // Given
        var repository = Mock.Of<IAirportReadonlyRepository>();

        var icaoCode = IcaoCode.Create("CYYJ");
        var iataCode = IataCode.Create("YYJ");
        var name = AirportName.Create("Victoria International Airport");
        var baseUtcOffset = BaseUtcOffset.Create(-7, 0);
        var airportLocation = AirportLocation.Create(Continent.NorthAmerica, "Canada", "British Columbia", "Victoria",
                                                                48.646900177, -123.426002502);

        // When
        var airport = await Airport.CreateAsync(
                        icaoCode, iataCode, name, baseUtcOffset, airportLocation, repository);

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
    public async Task Airport_ShouldCalculateDistanceToAnotherAirport_AsExpected(
        double originAirportLatitude, double originAirportLongitude,
        double destinationAirportLatitude, double destinationAirportLongitude,
        double expectedDistance)
    {
        // Given
        var originAirport = await FakeAirport.NewFakeAsync(originAirportLatitude, originAirportLongitude);
        var destinationAirport = await FakeAirport.NewFakeAsync(destinationAirportLatitude, destinationAirportLongitude);

        // When
        var distance = originAirport.GetDistanceTo(destinationAirport, LengthUnit.Kilometer);

        // Then
        distance.Should().Be(expectedDistance);
    }


    [Fact]
    public async Task Airport_ShouldThrowDuplicateIcaoCodeException_WhenIcaoCodeAlreadyExist()
    {
        // Given
        var icaoCodeString = "CYYJ";

        var repository = new Mock<IAirportReadonlyRepository>();

        repository
            .Setup(r => r.ExistAsync(icaoCodeString, CancellationToken.None))
            .ReturnsAsync(true);

        var icaoCode = IcaoCode.Create(icaoCodeString);
        var iataCode = IataCode.Create("YYJ");
        var name = AirportName.Create("Victoria International Airport");
        var baseUtcOffset = BaseUtcOffset.Create(-7, 0);
        var airportLocation = AirportLocation.Create(Continent.NorthAmerica, "Canada", "British Columbia", "Victoria",
            48.646900177, -123.426002502);

        // When
        var func = async () => await Airport.CreateAsync(
            icaoCode, iataCode, name, baseUtcOffset, airportLocation, repository.Object);

        // Then
        (await func.Should().ThrowExactlyAsync<DuplicateIcaoCodeException>())
            .And.Code.Should().Be(nameof(DuplicateIcaoCodeException));

    }
}
