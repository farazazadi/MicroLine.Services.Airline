using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;

public static class FakeCabinCrew
{
    public static CabinCrew NewFake(
        CabinCrewType type,
        Gender? gender = null,
        FullName fullName = null,
        Date birthDate = null,
        NationalId nationalId = null,
        PassportNumber passportNumber = null,
        Email email = null,
        ContactNumber contactNumber = null,
        Address address = null
        )
    {
        gender ??= FakeGender.PickRandom();
        fullName ??= FakeFullName.NewFake(gender.Value);
        birthDate ??= FakeDate.NewFake();
        nationalId ??= FakeNationalId.NewFake();
        passportNumber ??= FakePassportNumber.NewFake();
        email ??= FakeEmail.NewFake(fullName.FirstName, fullName.LastName);
        contactNumber ??= FakeContactNumber.NewFake();
        address ??= FakeAddress.NewFake();

        return CabinCrew.Create(type
                        , gender.Value
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
        return cabinCrewTypes.Select(type => NewFake(type)).ToList();
    }
}