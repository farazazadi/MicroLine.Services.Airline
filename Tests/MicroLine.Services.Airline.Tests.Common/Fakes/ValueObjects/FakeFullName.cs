
using Bogus;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class FakeFullName
{
    public static FullName NewFake(Domain.Common.Enums.Gender gender)
    {
        var faker = new Faker();

        var bogusGender = gender.ToBogusGender();

        var firstName = faker.Name.FirstName(bogusGender);
        var lastName = faker.Name.LastName(bogusGender);

        return FullName.Create(firstName, lastName);
    }
}
