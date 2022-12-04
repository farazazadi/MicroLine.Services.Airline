using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.Airports;

public class Airport : AggregateRoot
{
    public IcaoCode IcaoCode { get; private set; }
    public IataCode IataCode { get; private set; }
    public AirportName Name { get; private set; }
    public BaseUtcOffset BaseUtcOffset { get; private set; }
    public AirportLocation AirportLocation { get; private set; }

    private Airport() { }

    private Airport(IcaoCode icaoCode, IataCode iataCode, AirportName name, BaseUtcOffset baseUtcOffset, AirportLocation airportLocation)
    {
        IcaoCode = icaoCode;
        IataCode = iataCode;
        Name = name;
        BaseUtcOffset = baseUtcOffset;
        AirportLocation = airportLocation;
    }

    public static Airport Create(IcaoCode icaoCode, IataCode iataCode, AirportName name,
        BaseUtcOffset baseUtcOffset, AirportLocation airportLocation)
    {
        return new Airport(icaoCode, iataCode, name, baseUtcOffset, airportLocation);
    }


    public double GetDistanceTo(Airport destination, LengthUnit lengthUnit = LengthUnit.Kilometer)
    {

        var earthRadius = 6371000.0; // earth radius in meter

        var radiansOverDegrees = Math.PI / 180.0;

        var sourceLatitude = AirportLocation.Latitude;
        var sourceLongitude = AirportLocation.Longitude;

        var destinationLatitude = destination.AirportLocation.Latitude;
        var destinationLongitude = destination.AirportLocation.Longitude;


        var sourceLatitudeRadians = sourceLatitude * radiansOverDegrees;
        var sourceLongitudeRadians = sourceLongitude * radiansOverDegrees;
        var targetLatitudeRadians = destinationLatitude * radiansOverDegrees;
        var targetLongitudeRadians = destinationLongitude * radiansOverDegrees;

        var result =
            Math.Pow(Math.Sin((targetLatitudeRadians - sourceLatitudeRadians) / 2.0), 2.0)
            + Math.Cos(sourceLatitudeRadians)
            * Math.Cos(targetLatitudeRadians)
            * Math.Pow(Math.Sin((targetLongitudeRadians - sourceLongitudeRadians) / 2.0), 2.0);


        result = earthRadius * (2.0 * Math.Atan2(Math.Sqrt(result), Math.Sqrt(1.0 - result)));

        result = lengthUnit switch
        {
            LengthUnit.Kilometer => result / 1000,
            LengthUnit.Mile => result / 1609.344,
            _ => result
        };

        return result;


    }

}
