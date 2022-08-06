using System.Text.RegularExpressions;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Extensions;
using MicroLine.Services.Airline.Domain.Flights.Exceptions;

namespace MicroLine.Services.Airline.Domain.Flights;

public sealed class FlightNumber : ValueObject
{
    private readonly string _flightNumber;

    private FlightNumber(string flightNumber) => _flightNumber = flightNumber;

    public static FlightNumber Create(string flightNumber)
    {
        Validate(flightNumber);

        return new FlightNumber(flightNumber.Trim().ToUpperInvariant());
    }

    private static void Validate(string flightNumber)
    {
        if (flightNumber.IsNullOrEmpty())
            throw new InvalidFlightNumberException("FlightNumber could not be null or empty!");

        var trimmedFlightNumber = flightNumber.Trim();

        if (trimmedFlightNumber.Length is < 4 or > 8)
            throw new InvalidFlightNumberException("FlightNumber's length could not be greater than 8 or less than 4 characters!");

        var isValid = Regex.IsMatch(trimmedFlightNumber, "^[a-zA-Z]+[0-9]*$", RegexOptions.Compiled);

        if (!isValid)
            throw new InvalidFlightNumberException();

    }


    public static implicit operator string(FlightNumber flightNumber) => flightNumber.ToString();
    public static implicit operator FlightNumber(string flightNumber) => Create(flightNumber);

    public override string ToString() => _flightNumber;
}
