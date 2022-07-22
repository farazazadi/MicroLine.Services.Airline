using Bogus;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class AircraftRegistrationCode
{
    public static Domain.Aircraft.AircraftRegistrationCode NewFake()
    {
        var faker = new Faker();

        var code = faker.Random.String2(4, 7, RandomSelectionAllowedCharacters.DigitsAndUpperCaseLetters);

        return Domain.Aircraft.AircraftRegistrationCode.Create(code);
    }
}
