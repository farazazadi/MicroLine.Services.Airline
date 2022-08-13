using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Domain.Flights;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Application.Common.Contracts;

public interface IAirlineDbContext
{
    DbSet<Airport> Airports { get; }
    DbSet<Aircraft> Aircrafts { get; }
    DbSet<CabinCrew> CabinCrews { get; }
    DbSet<FlightCrew> FlightCrews { get; }
    DbSet<Flight> Flights { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}