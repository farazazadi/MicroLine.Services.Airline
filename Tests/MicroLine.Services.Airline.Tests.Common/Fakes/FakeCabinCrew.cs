using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;
using Moq;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;

public static class FakeCabinCrew
{
    public static async Task<CabinCrew> NewFakeAsync(CabinCrewType type)
    {
        var repository = Mock.Of<ICabinCrewReadonlyRepository>();

        var gender = FakeGender.PickRandom();
        var fullName = FakeFullName.NewFake(gender);
        var birthDate = FakeDate.NewFake();
        var nationalId = FakeNationalId.NewFake();
        var passportNumber = FakePassportNumber.NewFake();
        var email = FakeEmail.NewFake(fullName.FirstName, fullName.LastName);
        var contactNumber = FakeContactNumber.NewFake();
        var address = FakeAddress.NewFake();

        return await CabinCrew.CreateAsync(type
                        , gender
                        , fullName
                        , birthDate
                        , nationalId
                        , passportNumber
                        , email
                        , contactNumber
                        , address
                        ,repository);
    }


    public static async Task<List<CabinCrew>> NewFakeListAsync(params CabinCrewType[] cabinCrewTypes)
    {
        var cabinCrewList = new List<CabinCrew>();

        foreach (var cabinCrewType in cabinCrewTypes)
        {
            var cabinCrew = await NewFakeAsync(cabinCrewType);
            cabinCrewList.Add(cabinCrew);
        }

        return cabinCrewList;
    }
}