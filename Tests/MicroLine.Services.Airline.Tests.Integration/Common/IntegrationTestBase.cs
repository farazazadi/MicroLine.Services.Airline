using MapsterMapper;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Infrastructure.Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MicroLine.Services.Airline.Tests.Integration.Common;


[Collection(nameof(AirlineWebApplicationFactoryCollection))]
public abstract class IntegrationTestBase
{
    protected readonly AirlineWebApplicationFactory AirlineWebApplicationFactory;
    protected readonly HttpClient Client;
    protected readonly IMapper Mapper;
    private readonly DbContext _dbContext;
    private readonly RabbitMqClient _rabbitMqClient;

    protected IntegrationTestBase(AirlineWebApplicationFactory airlineWebApplicationFactory)
    {
        AirlineWebApplicationFactory = airlineWebApplicationFactory;

        Client = AirlineWebApplicationFactory.CreateClient();

        Mapper = AirlineWebApplicationFactory.Services.GetRequiredService<IMapper>();

        _dbContext = AirlineWebApplicationFactory.Services.GetRequiredService<DbContext>();

        _rabbitMqClient = AirlineWebApplicationFactory.Services.GetRequiredService<RabbitMqClient>();
    }

    protected async Task SaveAsync<TEntity>(TEntity entity) where TEntity : AggregateRoot
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    protected async Task SaveAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : AggregateRoot
    {
        await _dbContext.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }


    protected async Task<TEvent> GetEventFromRabbitMq<TEvent>() where TEvent : IntegrationEvent
    {
        return await _rabbitMqClient.Get<TEvent>();
    }
}
