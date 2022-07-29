
using Bogus;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class FakePassportNumber
{
    public static PassportNumber NewFake()
    {
        var faker = new Faker();

        var number = faker.Random.String2(6, 9, RandomSelectionAllowedCharacters.DigitsAndUpperCaseLetters);

        return PassportNumber.Create(number);
    }
}
