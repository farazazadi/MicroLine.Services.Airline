using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Repositories;

internal class CabinCrewReadonlyRepository : ICabinCrewReadonlyRepository
{
    private readonly IAirlineDbContext _dbContext;

    public CabinCrewReadonlyRepository(IAirlineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistAsync(PassportNumber passportNumber, CancellationToken token = default)
    {
        return await _dbContext.CabinCrews
            .AnyAsync(cabinCrew => cabinCrew.PassportNumber == passportNumber, token);
    }

    public async Task<bool> ExistAsync(NationalId nationalId, CancellationToken token = default)
    {
        return await _dbContext.CabinCrews
            .AnyAsync(cabinCrew => cabinCrew.NationalId == nationalId, token);
    }

    public async Task<CabinCrew> GetAsync(Id id, CancellationToken token = default)
    {
        return await _dbContext.CabinCrews.FindAsync(new object[] {id}, token);
    }
}
