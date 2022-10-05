using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;
using Moq;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;

public static class FakeFlightCrew
{
    public static async Task<FlightCrew> NewFakeAsync(FlightCrewType type)
    {
        var repository = Mock.Of<IFlightCrewReadonlyRepository>();

        var gender = FakeGender.PickRandom();
        var fullName = FakeFullName.NewFake(gender);
        var birthDate = FakeDate.NewFake();
        var nationalId = FakeNationalId.NewFake();
        var passportNumber = FakePassportNumber.NewFake();
        var email = FakeEmail.NewFake(fullName.FirstName, fullName.LastName);
        var contactNumber = FakeContactNumber.NewFake();
        var address = FakeAddress.NewFake();

        return await FlightCrew.CreateAsync(type
                        , gender
                        , fullName
                        , birthDate
                        , nationalId
                        , passportNumber
                        , email
                        , contactNumber
                        , address
                        , repository);
    }

    public static async Task<List<FlightCrew>> NewFakeListAsync(params FlightCrewType[] flightCrewTypes)
    {
        var flightCrewList = new List<FlightCrew>();

        foreach (var flightCrewType in flightCrewTypes)
        {
            var flightCrew = await NewFakeAsync(flightCrewType);
            flightCrewList.Add(flightCrew);
        }

        return flightCrewList;
    }
}