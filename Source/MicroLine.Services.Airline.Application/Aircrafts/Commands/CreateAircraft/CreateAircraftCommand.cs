using MediatR;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Aircrafts;

namespace MicroLine.Services.Airline.Application.Aircrafts.Commands.CreateAircraft;

public record CreateAircraftCommand(
    AircraftManufacturer Manufacturer,
    string Model,
    DateTime ManufactureDate,
    PassengerSeatingCapacityDto PassengerSeatingCapacity,
    string CruisingSpeed,
    string MaximumOperatingSpeed,
    string RegistrationCode

) : IRequest<AircraftDto>;