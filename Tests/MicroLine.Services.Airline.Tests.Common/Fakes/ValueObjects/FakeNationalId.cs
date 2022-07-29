
using Bogus;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class FakeNationalId
{
    public static NationalId NewFake()
    {
        var faker = new Faker();

        var id = faker.Random.String2(8, 20, RandomSelectionAllowedCharacters.DigitsAndUpperCaseLetters);

        return NationalId.Create(id);
    }
}