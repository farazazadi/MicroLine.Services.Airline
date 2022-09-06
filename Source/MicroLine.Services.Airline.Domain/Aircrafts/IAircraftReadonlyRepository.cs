namespace MicroLine.Services.Airline.Domain.Aircrafts;
public interface IAircraftReadonlyRepository
{
    Task<bool> ExistAsync(AircraftRegistrationCode registrationCode, CancellationToken token = default);
}
