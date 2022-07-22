
using Bogus;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class Address
{
    public static Domain.Common.ValueObjects.Address NewFake()
    {
        var faker = new Faker();

        var street = faker.Address.StreetName();
        var city = faker.Address.City();
        var state = faker.Address.State();
        var country = faker.Address.Country();
        var postalCode = faker.Address.ZipCode();

        return Domain.Common.ValueObjects.Address.Create(street, city, state, country, postalCode);
    }
}
