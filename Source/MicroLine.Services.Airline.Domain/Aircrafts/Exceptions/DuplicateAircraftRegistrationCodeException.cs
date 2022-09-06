using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Aircrafts.Exceptions;

public class DuplicateAircraftRegistrationCodeException : DomainException
{
    public override string Code => nameof(DuplicateAircraftRegistrationCodeException);

    public DuplicateAircraftRegistrationCodeException(string registrationCode) : base($"An aircraft with same RegistrationCode ({registrationCode}) already exist!")
    {
    }
}
