
using Bogus;
using MicroLine.Services.Airline.Domain.Aircraft;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class AircraftModel
{
    public static Domain.Aircraft.AircraftModel NewFake(AircraftManufacturer manufacturer)
    {
        var faker = new Faker();

        var airbusModels = new[] { "A350", "A380", "A330", "A321", "A320", "A300" };
        var boeingModels = new[] { "787", "777", "757", "747", "737 MAX", "707" };
        var lockheedMartinModels = new[] { "LM-100J"};
        var bombardierModels = new[] { "CHALLENGER 300", "CHALLENGER 605", "GLOBAL 5000", "GLOBAL 6000" };
        var embraerModels = new[] { " E175-E2", "E195", "ERJ135"};
        var tupoloevModels = new[] { "Tu-154", "Tu-334"};


        var model = manufacturer switch
        {
            AircraftManufacturer.Airbus => faker.PickRandom(airbusModels),
            AircraftManufacturer.Boeing => faker.PickRandom(boeingModels),
            AircraftManufacturer.LockheedMartin => faker.PickRandom(lockheedMartinModels),
            AircraftManufacturer.Bombardier => faker.PickRandom(bombardierModels),
            AircraftManufacturer.Embraer => faker.PickRandom(embraerModels),
            AircraftManufacturer.Tupoloev => faker.PickRandom(tupoloevModels),
            _ => throw new NotImplementedException()
        };

        return Domain.Aircraft.AircraftModel.Create(model);
    }
}
