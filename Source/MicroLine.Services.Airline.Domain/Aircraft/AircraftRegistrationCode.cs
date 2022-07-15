using MicroLine.Services.Airline.Domain.Aircraft.Exceptions;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Extensions;

namespace MicroLine.Services.Airline.Domain.Aircraft;

public sealed class AircraftRegistrationCode : ValueObject
{
    private readonly string _registerCode;

    private AircraftRegistrationCode(string registerCode) => _registerCode = registerCode;

    public static AircraftRegistrationCode Create(string registerCode)
    {
        Validate(registerCode);

        return new AircraftRegistrationCode(registerCode.Trim());
    }

    private static void Validate(string registerCode)
    {
        if (registerCode.IsNullOrEmpty())
            throw new InvalidAircraftRegistrationCodeException("Aircraft registration code could not be null or empty!");

        if(registerCode.Trim().Length is < 3 or > 20)
            throw new InvalidAircraftRegistrationCodeException("Aircraft registration code's length could not be greater than 20 or less than 3 characters!");
    }

    public static implicit operator string(AircraftRegistrationCode registrationCode) => registrationCode.ToString();
    public static implicit operator AircraftRegistrationCode(string registrationCode) => Create(registrationCode);

    public override string ToString() => _registerCode;
}