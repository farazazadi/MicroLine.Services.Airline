
using Bogus;
using MicroLine.Services.Airline.Domain.Common.Extensions;
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


        while (!firstName.AreAllCharactersLetter())
            firstName = faker.Name.FirstName(bogusGender);

        while (!lastName.AreAllCharactersLetter())
            lastName = faker.Name.LastName(bogusGender);


        while (firstName.Length < 3)
            firstName += "a";

        while (lastName.Length < 3)
            lastName += "a";

        return FullName.Create(firstName, lastName);
    }
}
