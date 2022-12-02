using System.Linq.Expressions;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Flights;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Repositories;

internal class FlightReadonlyRepository : IFlightReadonlyRepository
{
    private readonly IAirlineDbContext _dbContext;

    public FlightReadonlyRepository(IAirlineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Flight> GetAsync(Expression<Func<Flight, bool>> predicate, CancellationToken token = default)
    {
        return await _dbContext.Flights
            .AsNoTracking()
            .Include(flight => flight.OriginAirport)
            .Include(flight => flight.DestinationAirport)
            .Include(flight => flight.Aircraft)
            .Include(flight => flight.FlightCrewMembers)
            .Include(flight => flight.CabinCrewMembers)
            .FirstOrDefaultAsync(predicate, token);
    }

    public async Task<IReadOnlyList<Flight>> GetAllAsync(Expression<Func<Flight, bool>> predicate, CancellationToken token = default)
    {
        return await _dbContext.Flights
            .AsNoTrackingWithIdentityResolution()
            .Include(flight => flight.OriginAirport)
            .Include(flight => flight.DestinationAirport)
            .Include(flight => flight.Aircraft)
            .Include(flight => flight.FlightCrewMembers)
            .Include(flight => flight.CabinCrewMembers)
            .Where(predicate)
            .ToListAsync(token);
    }

    public async Task<IReadOnlyList<Flight>> GetAllAsync(CancellationToken token = default)
    {
        return await _dbContext.Flights
            .AsNoTrackingWithIdentityResolution()
            .Include(flight => flight.OriginAirport)
            .Include(flight => flight.DestinationAirport)
            .Include(flight => flight.Aircraft)
            .Include(flight => flight.FlightCrewMembers)
            .Include(flight => flight.CabinCrewMembers)
            .ToListAsync(token);
    }
}