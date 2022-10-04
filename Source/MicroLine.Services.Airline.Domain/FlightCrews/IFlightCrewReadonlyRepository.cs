
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.FlightCrews;
public interface IFlightCrewReadonlyRepository
{
    Task<bool> ExistAsync(PassportNumber passportNumber, CancellationToken token = default);
    Task<bool> ExistAsync(NationalId nationalId, CancellationToken token = default);
    Task<FlightCrew> GetAsync(Id id, CancellationToken token = default);
}
