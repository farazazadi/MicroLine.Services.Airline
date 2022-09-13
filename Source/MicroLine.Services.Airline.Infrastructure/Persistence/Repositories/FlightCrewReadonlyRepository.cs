using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Repositories;

internal class FlightCrewReadonlyRepository : IFlightCrewReadonlyRepository
{
    private readonly IAirlineDbContext _dbContext;

    public FlightCrewReadonlyRepository(IAirlineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistAsync(PassportNumber passportNumber, CancellationToken token = default)
    {
        return await _dbContext.FlightCrews
            .AnyAsync(flightCrew => flightCrew.PassportNumber == passportNumber, token);
    }
}
