
using Bogus;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class FakeAddress
{
    public static Address NewFake()
    {
        var faker = new Faker();

        var street = faker.Address.StreetName();
        var city = faker.Address.City();
        var state = faker.Address.State();
        var country = faker.Address.Country();
        var postalCode = faker.Address.ZipCode();

        return Address.Create(street, city, state, country, postalCode);
    }
}
