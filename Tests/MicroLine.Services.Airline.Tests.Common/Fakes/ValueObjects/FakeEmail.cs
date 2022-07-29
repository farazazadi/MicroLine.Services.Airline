
using Bogus;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class FakeEmail
{
    public static Email NewFake(string firstName, string lastName)
    {
        var faker = new Faker();

        var email = faker.Internet.Email(firstName, lastName);

        return Email.Create(email);
    }
}
