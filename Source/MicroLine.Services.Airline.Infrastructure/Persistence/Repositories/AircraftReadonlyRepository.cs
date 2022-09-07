using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Repositories;
internal class AircraftReadonlyRepository : IAircraftReadonlyRepository
{
    private readonly IAirlineDbContext _dbContext;

    public AircraftReadonlyRepository(IAirlineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistAsync(AircraftRegistrationCode registrationCode, CancellationToken token = default)
    {
        return await _dbContext.Aircrafts.AnyAsync(
            aircraft => aircraft.RegistrationCode == registrationCode, token);
    }

    public async Task<Aircraft> GetAsync(Id id, CancellationToken token = default)
    {
        return await _dbContext.Aircrafts.FindAsync(new object[] {id}, token);
    }
}
