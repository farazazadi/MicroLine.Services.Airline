using MicroLine.Services.Airline.Domain.Common;

namespace MicroLine.Services.Airline.Domain.Airport;

public class Airport : AggregateRoot
{
    public IcaoCode IcaoCode { get; }
    public IataCode IataCode { get; }
    public AirportName Name { get; }
    public AirportLocation AirportLocation { get; }

    private Airport(IcaoCode icaoCode, IataCode iataCode, AirportName name, AirportLocation airportLocation)
    {
        IcaoCode = icaoCode;
        IataCode = iataCode;
        Name = name;
        AirportLocation = airportLocation;
    }


    public static Airport Create(IcaoCode icaoCode, IataCode iataCode, AirportName name, AirportLocation airportLocation)
    {
        return new (icaoCode, iataCode, name, airportLocation);
    }
}
