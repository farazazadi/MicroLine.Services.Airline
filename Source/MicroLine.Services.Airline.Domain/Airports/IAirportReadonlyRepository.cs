using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.Airports;

public interface IAirportReadonlyRepository
{
    Task<bool> ExistAsync(IcaoCode icaoCode, CancellationToken token = default);
    Task<Airport> GetAsync(Id id, CancellationToken token = default);
}