using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.CabinCrews;
public interface ICabinCrewReadonlyRepository
{
    Task<bool> ExistAsync(PassportNumber passportNumber, CancellationToken token = default);
    Task<bool> ExistAsync(NationalId nationalId, CancellationToken token = default);
    Task<CabinCrew> GetAsync(Id id, CancellationToken token = default);
}
