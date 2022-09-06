using Bogus;
using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using Moq;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;

public static class FakeAircraft
{
    public static async Task<Aircraft> NewFakeAsync(AircraftManufacturer manufacturer,
        int? economyClassCapacity = null,
        int? businessClassCapacity = null,
        int? firstClassCapacity = null)
    {
        var repository = Mock.Of<IAircraftReadonlyRepository>();

        var faker = new Faker();

        var model = NewFakeAircraftModel(manufacturer, faker);
        var manufactureDate = ValueObjects.FakeDate.NewFake();
        var maximumSeatingCapacity = NewFakePassengerSeatingCapacity(economyClassCapacity, businessClassCapacity, firstClassCapacity);
        var cruisingSpeed = NewFakeAircraftCruisingSpeed();
        var maximumOperatingSpeed = NewFakeAircraftMaximumOperatingSpeed();
        var registrationCode = NewFakeAircraftRegistrationCode(faker);

        var aircraft = await Aircraft.CreateAsync(manufacturer
            , model
            , manufactureDate
            , maximumSeatingCapacity
            , cruisingSpeed
            , maximumOperatingSpeed
            , registrationCode
            , repository
        );

        return aircraft;
    }


    private static AircraftModel NewFakeAircraftModel(AircraftManufacturer manufacturer, Faker faker)
    {
        var airbusModels = new[] { "A350", "A380", "A330", "A321", "A320", "A300" };
        var boeingModels = new[] { "787", "777", "757", "747", "737 MAX", "707" };
        var lockheedMartinModels = new[] { "LM-100J" };
        var bombardierModels = new[] { "CHALLENGER 300", "CHALLENGER 605", "GLOBAL 5000", "GLOBAL 6000" };
        var embraerModels = new[] { " E175-E2", "E195", "ERJ135" };
        var tupoloevModels = new[] { "Tu-154", "Tu-334" };


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

        return AircraftModel.Create(model);
    }

    private static PassengerSeatingCapacity NewFakePassengerSeatingCapacity(
        int? economyClassCapacity = null, int? businessClassCapacity = null, int? firstClassCapacity = null)
    {
        economyClassCapacity ??= Random.Shared.Next(100, 150);
        businessClassCapacity ??= Random.Shared.Next(30, 50);
        firstClassCapacity ??= Random.Shared.Next(10, 20);

        return PassengerSeatingCapacity.Create(economyClassCapacity.Value, businessClassCapacity.Value, firstClassCapacity.Value);
    }

    private static Speed NewFakeAircraftCruisingSpeed()
    {
        var speed = Random.Shared.Next(800, 830);

        return Speed.Create(speed, Speed.UnitOfSpeed.KilometresPerHour);
    }

    private static Speed NewFakeAircraftMaximumOperatingSpeed()
    {
        var speed = Random.Shared.Next(850, 880);

        return Speed.Create(speed, Speed.UnitOfSpeed.KilometresPerHour);
    }

    private static AircraftRegistrationCode NewFakeAircraftRegistrationCode(Faker faker)
    {
        var code = faker.Random.String2(4, 7, RandomSelectionAllowedCharacters.DigitsAndUpperCaseLetters);

        return AircraftRegistrationCode.Create(code);
    }

}