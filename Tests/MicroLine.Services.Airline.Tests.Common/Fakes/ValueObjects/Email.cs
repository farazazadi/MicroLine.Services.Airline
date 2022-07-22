
using Bogus;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class Email
{
    public static Domain.Common.ValueObjects.Email NewFake(string firstName, string lastName)
    {
        var faker = new Faker();

        var email = faker.Internet.Email(firstName, lastName);

        return Domain.Common.ValueObjects.Email.Create(email);
    }
}
