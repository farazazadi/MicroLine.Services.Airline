using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.CabinCrews;
public interface ICabinCrewReadonlyRepository
{
    Task<bool> ExistAsync(PassportNumber passportNumber, CancellationToken token = default);
}
