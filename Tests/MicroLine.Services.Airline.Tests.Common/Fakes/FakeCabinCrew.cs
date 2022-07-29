
using MicroLine.Services.Airline.Domain.CabinCrew;
using MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;

public static class FakeCabinCrew
{
    public static CabinCrew NewFake(CabinCrewType type)
    {
        var gender = FakeGender.PickRandom();
        var fullName = FakeFullName.NewFake(gender);
        var birthDate = FakeDate.NewFake();
        var nationalId = FakeNationalId.NewFake();
        var passportNumber = FakePassportNumber.NewFake();
        var email = FakeEmail.NewFake(fullName.FirstName, fullName.LastName);
        var contactNumber = FakeContactNumber.NewFake();
        var address = FakeAddress.NewFake();

        return CabinCrew.Create(type
                        , gender
                        , fullName
                        , birthDate
                        , nationalId
                        , passportNumber
                        , email
                        , contactNumber
                        , address);
    }


    public static List<CabinCrew> NewFakeList(params CabinCrewType[] cabinCrewTypes)
    {
        return cabinCrewTypes.Select(NewFake).ToList();
    }
}