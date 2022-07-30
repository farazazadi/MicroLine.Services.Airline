using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.Aircraft;

public class Aircraft : AggregateRoot
{
    public AircraftManufacturer Manufacturer { get; }
    public AircraftModel Model { get; }
    public Date ManufactureDate { get; }
    public PassengerSeatingCapacity PassengerSeatingCapacity { get; }
    public Speed CruisingSpeed { get; }
    public Speed MaximumOperatingSpeed { get; }
    public AircraftRegistrationCode RegistrationCode { get; }

    private Aircraft(AircraftManufacturer manufacturer, AircraftModel model, Date manufactureDate,
        PassengerSeatingCapacity passengerSeatingCapacity, Speed cruisingSpeed, Speed maximumOperatingSpeed,
        AircraftRegistrationCode registrationCode)
    {
        Manufacturer = manufacturer;
        Model = model;
        ManufactureDate = manufactureDate;
        PassengerSeatingCapacity = passengerSeatingCapacity;
        CruisingSpeed = cruisingSpeed;
        MaximumOperatingSpeed = maximumOperatingSpeed;
        RegistrationCode = registrationCode;
    }

    public static Aircraft Create(AircraftManufacturer manufacturer, AircraftModel model, Date manufactureDate,
                                    PassengerSeatingCapacity passengerSeatingCapacity, Speed cruisingSpeed, Speed maximumOperatingSpeed,
                                    AircraftRegistrationCode registrationCode)
    {

        return new Aircraft(manufacturer, model, manufactureDate,
            passengerSeatingCapacity, cruisingSpeed, maximumOperatingSpeed,
            registrationCode);
    }

}