using MicroLine.Services.Airline.Domain.Common;

namespace MicroLine.Services.Airline.Application.Common.Contracts;

public interface IEventDispatcher
{
    Task DispatchAsync(DomainEvent domainEvent, CancellationToken token);
}