using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.Aircraft;

public class Aircraft : AggregateRoot
{
    public AircraftManufacturer Manufacturer { get; }
    public AircraftModel Model { get; }
    public Date ManufactureDate { get; }
    public AircraftMaximumSeatingCapacity MaximumSeatingCapacity { get; }
    public Speed CruisingSpeed { get; }
    public Speed MaximumOperatingSpeed { get; }
    public AircraftRegistrationCode RegistrationCode { get; }

    private Aircraft(AircraftManufacturer manufacturer, AircraftModel model, Date manufactureDate,
        int maximumSeatingCapacity, Speed cruisingSpeed, Speed maximumOperatingSpeed,
        AircraftRegistrationCode registrationCode)
    {
        Manufacturer = manufacturer;
        Model = model;
        ManufactureDate = manufactureDate;
        MaximumSeatingCapacity = maximumSeatingCapacity;
        CruisingSpeed = cruisingSpeed;
        MaximumOperatingSpeed = maximumOperatingSpeed;
        RegistrationCode = registrationCode;
    }

    public static Aircraft Create(AircraftManufacturer manufacturer, AircraftModel model, Date manufactureDate,
                                    AircraftMaximumSeatingCapacity maximumSeatingCapacity, Speed cruisingSpeed, Speed maximumOperatingSpeed,
                                    AircraftRegistrationCode registrationCode)
    {

        return new Aircraft(manufacturer, model, manufactureDate,
            maximumSeatingCapacity, cruisingSpeed, maximumOperatingSpeed,
            registrationCode);
    }

}