
using Bogus;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class ContactNumber
{
    public static Domain.Common.ValueObjects.ContactNumber NewFake()
    {
        var faker = new Faker();

        var number = faker.Phone.PhoneNumber("+1###########");

        return Domain.Common.ValueObjects.ContactNumber.Create(number);
    }
}