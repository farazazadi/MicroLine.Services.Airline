
using Bogus;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class FakeContactNumber
{
    public static ContactNumber NewFake()
    {
        var faker = new Faker();

        var number = faker.Phone.PhoneNumber("+1###########");

        return ContactNumber.Create(number);
    }
}