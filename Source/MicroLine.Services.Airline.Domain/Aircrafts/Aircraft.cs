using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.Aircrafts;

public class Aircraft : AggregateRoot
{
    public AircraftManufacturer Manufacturer { get; private set; }
    public AircraftModel Model { get; private set; }
    public Date ManufactureDate { get; private set; }
    public PassengerSeatingCapacity PassengerSeatingCapacity { get; private set; }
    public Speed CruisingSpeed { get; private set; }
    public Speed MaximumOperatingSpeed { get; private set; }
    public AircraftRegistrationCode RegistrationCode { get; private set; }

    private Aircraft() { }

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