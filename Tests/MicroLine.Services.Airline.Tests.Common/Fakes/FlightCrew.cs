using MicroLine.Services.Airline.Domain.FlightCrew;
using MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;

public static class FlightCrew
{
    public static Domain.FlightCrew.FlightCrew NewFake(FlightCrewType type)
    {
        var gender = Gender.PickRandom();
        var fullName = FullName.NewFake(gender);
        var birthDate = BirthDate.NewFake();
        var nationalId = NationalId.NewFake();
        var passportNumber = PassportNumber.NewFake();
        var email = Email.NewFake(fullName.FirstName, fullName.LastName);
        var contactNumber = ContactNumber.NewFake();
        var address = Address.NewFake();

        return Domain.FlightCrew.FlightCrew.Create(type
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

    public static List<Domain.FlightCrew.FlightCrew> NewFakeList(params FlightCrewType[] flightCrewTypes)
    {
        return flightCrewTypes.Select(NewFake).ToList();
    }
}