
using Bogus;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class FullName
{
    public static Domain.Common.ValueObjects.FullName NewFake(Domain.Common.Enums.Gender gender)
    {
        var faker = new Faker();

        var bogusGender = gender.ToBogusGender();

        var firstName = faker.Name.FirstName(bogusGender);
        var lastName = faker.Name.LastName(bogusGender);

        return Domain.Common.ValueObjects.FullName.Create(firstName, lastName);
    }
}
