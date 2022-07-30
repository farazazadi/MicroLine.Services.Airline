using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;

public static class FakeFlightCrew
{
    public static FlightCrew NewFake(FlightCrewType type)
    {
        var gender = FakeGender.PickRandom();
        var fullName = FakeFullName.NewFake(gender);
        var birthDate = FakeDate.NewFake();
        var nationalId = FakeNationalId.NewFake();
        var passportNumber = FakePassportNumber.NewFake();
        var email = FakeEmail.NewFake(fullName.FirstName, fullName.LastName);
        var contactNumber = FakeContactNumber.NewFake();
        var address = FakeAddress.NewFake();

        return FlightCrew.Create(type
                        , gender
                        , fullName
                        , birthDate
                        , nationalId
                        , passportNumber
                        , email
                        , contactNumber
                        , address);
    }

    public static List<FlightCrew> NewFakeList(params FlightCrewType[] flightCrewTypes)
    {
        return flightCrewTypes.Select(NewFake).ToList();
    }
}