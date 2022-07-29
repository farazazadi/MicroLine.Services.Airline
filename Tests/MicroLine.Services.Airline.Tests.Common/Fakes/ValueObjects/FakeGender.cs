
using Bogus;
using MicroLine.Services.Airline.Domain.Common.Enums;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class FakeGender
{
    public static Gender PickRandom()
    {
        return new Faker().PickRandom<Gender>();
    }

    internal static Bogus.DataSets.Name.Gender ToBogusGender(this Gender gender)
    {
        return gender switch
        {
            Gender.Female => Bogus.DataSets.Name.Gender.Female,
            Gender.Male => Bogus.DataSets.Name.Gender.Male,
            Gender.Other => Bogus.DataSets.Name.Gender.Female,
            _ => throw new NotImplementedException(),
        };
    }
}