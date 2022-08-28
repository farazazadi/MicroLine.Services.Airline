namespace MicroLine.Services.Airline.Domain.Airports;

public interface IAirportReadonlyRepository
{
    Task<bool> ExistAsync(IcaoCode icaoCode, CancellationToken token = default);
}