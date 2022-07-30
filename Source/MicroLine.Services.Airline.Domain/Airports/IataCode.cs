using MicroLine.Services.Airline.Domain.Airports.Exceptions;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Extensions;

namespace MicroLine.Services.Airline.Domain.Airports;

public class IataCode : ValueObject
{
    private readonly string _iataCode;

    private IataCode(string iataCode) => _iataCode = iataCode;

    public static IataCode Create(string iataCode)
    {
        Validate(iataCode);

        return new IataCode(iataCode.ToUpperInvariant());
    }

    private static void Validate(string iataCode)
    {
        if (iataCode.IsNullOrEmpty())
            throw new InvalidIataCodeException("IataCode can not be null or empty!");

        if (!iataCode.AreAllCharactersEnglishLetter())
            throw new InvalidIataCodeException("IataCode can only contain letter characters!");

        if (!iataCode.HasValidLength(3))
            throw new InvalidIataCodeException("IataCode should be 4 characters!");
    }

    public static implicit operator IataCode(string iataCode) => Create(iataCode);
    public static implicit operator string(IataCode iataCode) => iataCode.ToString();

    public override string ToString() => _iataCode;
}