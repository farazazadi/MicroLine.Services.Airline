
using MicroLine.Services.Airline.Domain.CabinCrew;
using MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;

public static class CabinCrew
{
    public static Domain.CabinCrew.CabinCrew NewFake(CabinCrewType type)
    {
        var gender = Gender.PickRandom();
        var fullName = FullName.NewFake(gender);
        var birthDate = BirthDate.NewFake();
        var nationalId = NationalId.NewFake();
        var passportNumber = PassportNumber.NewFake();
        var email = Email.NewFake(fullName.FirstName, fullName.LastName);
        var contactNumber = ContactNumber.NewFake();
        var address = Address.NewFake();

        return Domain.CabinCrew.CabinCrew.Create(type
            , gender
            , fullName
            , birthDate
            , nationalId
            , passportNumber
            , email
            , contactNumber
            , address
            );
    }


    public static List<Domain.CabinCrew.CabinCrew> NewFakeList(params CabinCrewType[] cabinCrewTypes)
    {
        return cabinCrewTypes.Select(NewFake).ToList();
    }
}