
using Bogus;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Tests.Common.Extensions;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class FakeAddress
{
    public static Address NewFake()
    {
        var faker = new Faker();

        var street = faker.Address.StreetName().Truncate(50);
        var city = faker.Address.City().Truncate(50);
        var state = faker.Address.State().Truncate(50);
        var country = faker.Address.Country().Truncate(50);
        var postalCode = faker.Address.ZipCode().Truncate(50);

        return Address.Create(street, city, state, country, postalCode);
    }
}
