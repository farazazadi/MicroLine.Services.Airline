using MicroLine.Services.Airline.Domain.Aircraft.Exceptions;
using MicroLine.Services.Airline.Domain.Common;

namespace MicroLine.Services.Airline.Domain.Aircraft;

public class AircraftMaximumSeatingCapacity : ValueObject
{
    private readonly int _capacity;

    private AircraftMaximumSeatingCapacity(int aircraftMaximumSeatingCapacity) => _capacity = aircraftMaximumSeatingCapacity;

    public static AircraftMaximumSeatingCapacity Create(int aircraftMaximumSeatingCapacity)
    {
        Validate(aircraftMaximumSeatingCapacity);

        return new AircraftMaximumSeatingCapacity(aircraftMaximumSeatingCapacity);
    }

    private static void Validate(int aircraftMaximumSeatingCapacity)
    {

        if (aircraftMaximumSeatingCapacity < 2)
            throw new InvalidAircraftMaximumSeatingCapacityException(aircraftMaximumSeatingCapacity);
    }


    public static implicit operator int(AircraftMaximumSeatingCapacity aircraftMaximumSeatingCapacity) => int.Parse(aircraftMaximumSeatingCapacity.ToString());
    public static implicit operator AircraftMaximumSeatingCapacity(int aircraftMaximumSeatingCapacity) => Create(aircraftMaximumSeatingCapacity);

    public override string ToString() => _capacity.ToString();

}