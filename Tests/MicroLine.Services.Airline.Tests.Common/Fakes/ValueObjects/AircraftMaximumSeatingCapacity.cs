namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class AircraftMaximumSeatingCapacity
{
    public static Domain.Aircraft.AircraftMaximumSeatingCapacity NewFake(int? capacity = null)
    {
        var tempCapacity = capacity ?? Random.Shared.Next(100, 250);

        return Domain.Aircraft.AircraftMaximumSeatingCapacity.Create(tempCapacity);
    }
}
