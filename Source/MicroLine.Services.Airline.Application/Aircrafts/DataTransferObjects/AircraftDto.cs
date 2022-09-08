using MicroLine.Services.Airline.Application.Common.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Aircrafts;

namespace MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;

public record AircraftDto(
    string Id,
    AircraftManufacturer Manufacturer,
    string Model,
    DateTime ManufactureDate,
    PassengerSeatingCapacityDto PassengerSeatingCapacity,
    string CruisingSpeed,
    string MaximumOperatingSpeed,
    string RegistrationCode,
    EntityAuditingDetailsDto AuditingDetails
);