namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class AircraftMaximumOperatingSpeed
{
    public static Domain.Common.ValueObjects.Speed NewFake()
    {
        var speed = Random.Shared.Next(850, 880);

        return Domain.Common.ValueObjects.Speed
            .Create(speed, Domain.Common.ValueObjects.Speed.UnitOfSpeed.KilometresPerHour);
    }
}