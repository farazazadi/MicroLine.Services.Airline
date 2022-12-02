using System.Linq.Expressions;

namespace MicroLine.Services.Airline.Domain.Flights;
public interface IFlightReadonlyRepository
{
    Task<Flight> GetAsync(Expression<Func<Flight, bool>> predicate, CancellationToken token = default);
    Task<IReadOnlyList<Flight>> GetAllAsync(Expression<Func<Flight, bool>> predicate, CancellationToken token = default);
    Task<IReadOnlyList<Flight>> GetAllAsync(CancellationToken token = default);
}
