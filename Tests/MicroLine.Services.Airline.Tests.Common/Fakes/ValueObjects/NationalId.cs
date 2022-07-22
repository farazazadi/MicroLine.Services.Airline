
using Bogus;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class NationalId
{
    public static Domain.Common.ValueObjects.NationalId NewFake()
    {
        var faker = new Faker();

        var id = faker.Random.String2(8, 20, RandomSelectionAllowedCharacters.DigitsAndUpperCaseLetters);

        return Domain.Common.ValueObjects.NationalId.Create(id);
    }
}