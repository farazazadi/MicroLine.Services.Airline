using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Airports;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Repositories;

internal class AirportReadonlyRepository : IAirportReadonlyRepository
{
    private readonly IAirlineDbContext _dbContext;

    public AirportReadonlyRepository(IAirlineDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> ExistAsync(IcaoCode icaoCode, CancellationToken token = default)
    {
        return await _dbContext.Airports.AnyAsync(airport => airport.IcaoCode == icaoCode, token);
    }
}
