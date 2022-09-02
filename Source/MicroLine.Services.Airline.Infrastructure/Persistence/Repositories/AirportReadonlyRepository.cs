using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
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

    public async Task<Airport> GetAsync(Id id, CancellationToken token = default)
    {
        return await _dbContext.Airports.FindAsync(new object[] {id}, token);
    }

    public async Task<IReadOnlyList<Airport>> GetAllAsync(CancellationToken token = default)
    {
        return await _dbContext.Airports
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync(token);
    }
}
