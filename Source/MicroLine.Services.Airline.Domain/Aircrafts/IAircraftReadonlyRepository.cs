using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.Aircrafts;
public interface IAircraftReadonlyRepository
{
    Task<bool> ExistAsync(AircraftRegistrationCode registrationCode, CancellationToken token = default);
    Task<Aircraft> GetAsync(Id id, CancellationToken token = default);
}
