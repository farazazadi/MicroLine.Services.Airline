using MapsterMapper;
using MicroLine.Services.Airline.Tests.Integration.Common.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace MicroLine.Services.Airline.Tests.Integration.Common;


[Collection(nameof(AirlineWebApplicationFactoryFixture))]
public abstract class IntegrationTestBase
{
    protected readonly AirlineWebApplicationFactory AirlineWebApplicationFactory;
    protected readonly HttpClient Client;
    protected readonly IMapper Mapper;

    protected IntegrationTestBase(AirlineWebApplicationFactory airlineWebApplicationFactory)
    {
        AirlineWebApplicationFactory = airlineWebApplicationFactory;

        Client = AirlineWebApplicationFactory.CreateClient();

        Mapper = AirlineWebApplicationFactory.Services.GetRequiredService<IMapper>();
    }

}
