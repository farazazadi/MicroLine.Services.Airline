using MicroLine.Services.Airline.Domain.Airport.Exceptions;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Extensions;

namespace MicroLine.Services.Airline.Domain.Airport;

public class IcaoCode : ValueObject
{
    private readonly string _icaoCode;

    private IcaoCode(string icaoCode) => _icaoCode = icaoCode;

    public static IcaoCode Create(string icaoCode)
    {
        Validate(icaoCode);

        return new IcaoCode(icaoCode.ToUpperInvariant());
    }

    private static void Validate(string icaoCode)
    {
        if (icaoCode.IsNullOrEmpty())
            throw new InvalidIcaoCodeException("IcaoCode can not be null or empty!");

        if(!icaoCode.AreAllCharactersEnglishLetter())
            throw new InvalidIcaoCodeException("IcaoCode can only contain letter characters!");

        if (!icaoCode.HasValidLength(4))
            throw new InvalidIcaoCodeException("IcaoCode should be 4 characters!");
    }

    public static implicit operator IcaoCode (string icaoCode)=> Create(icaoCode);
    public static implicit operator string (IcaoCode icaoCode) => icaoCode.ToString();

    public override string ToString()=> _icaoCode;
}