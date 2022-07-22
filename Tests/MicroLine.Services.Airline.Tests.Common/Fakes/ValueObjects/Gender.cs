
using Bogus;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class Gender
{
    public static Domain.Common.Enums.Gender PickRandom()
    {
        return new Faker().PickRandom<Domain.Common.Enums.Gender>();
    }

    internal static Bogus.DataSets.Name.Gender ToBogusGender(this Domain.Common.Enums.Gender gender)
    {
        return gender switch
        {
            Domain.Common.Enums.Gender.Female => Bogus.DataSets.Name.Gender.Female,
            Domain.Common.Enums.Gender.Male => Bogus.DataSets.Name.Gender.Male,
            Domain.Common.Enums.Gender.Other => Bogus.DataSets.Name.Gender.Female,
            _ => throw new NotImplementedException(),
        };
    }
}