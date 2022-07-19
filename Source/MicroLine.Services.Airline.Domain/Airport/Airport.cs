using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.Airport;

public class Airport : AggregateRoot
{
    public IcaoCode IcaoCode { get; }
    public IataCode IataCode { get; }
    public AirportName Name { get; }
    public BaseUtcOffset BaseUtcOffset { get; }
    public AirportLocation AirportLocation { get; }

    private Airport(IcaoCode icaoCode, IataCode iataCode, AirportName name, BaseUtcOffset baseUtcOffset, AirportLocation airportLocation)
    {
        IcaoCode = icaoCode;
        IataCode = iataCode;
        Name = name;
        BaseUtcOffset = baseUtcOffset;
        AirportLocation = airportLocation;
    }


    public static Airport Create(IcaoCode icaoCode, IataCode iataCode, AirportName name, BaseUtcOffset baseUtcOffset, AirportLocation airportLocation)
    {
        return new (icaoCode, iataCode, name, baseUtcOffset, airportLocation);
    }
}
