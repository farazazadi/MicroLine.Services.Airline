using MicroLine.Services.Airline.Domain.Airport.Exceptions;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Extensions;

namespace MicroLine.Services.Airline.Domain.Airport;

public class AirportName : ValueObject
{
    private readonly string _airportName;

    private AirportName(string airportName) => _airportName = airportName;

    public static AirportName Create(string airportName)
    {
        Validate(airportName);

        return new AirportName(airportName.Trim());
    }

    private static void Validate(string airportName)
    {
        if (airportName.IsNullOrEmpty())
            throw new InvalidAirportNameException("AirportName can not be null or empty!");


        if (!airportName.HasValidLength(4,60))
            throw new InvalidAirportNameException("AirportName should be greater than 3 and less than 60 characters!");
    }

    public static implicit operator AirportName(string airportName) => Create(airportName);
    public static implicit operator string(AirportName airportName) => airportName.ToString();

    public override string ToString() => _airportName;
}