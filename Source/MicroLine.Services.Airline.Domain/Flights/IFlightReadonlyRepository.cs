using System.Linq.Expressions;

namespace MicroLine.Services.Airline.Domain.Flights;
public interface IFlightReadonlyRepository
{
    Task<Flight> GetAsync(Expression<Func<Flight, bool>> predicate, CancellationToken token = default);
}
