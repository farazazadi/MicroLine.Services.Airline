
using Bogus;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class PassportNumber
{
    public static Domain.Common.ValueObjects.PassportNumber NewFake()
    {
        var faker = new Faker();

        var number = faker.Random.String2(6, 9, RandomSelectionAllowedCharacters.DigitsAndUpperCaseLetters);

        return Domain.Common.ValueObjects.PassportNumber.Create(number);
    }
}
