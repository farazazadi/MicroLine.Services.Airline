
namespace MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;

public record PassengerSeatingCapacityDto(
    int EconomyClassCapacity,
    int BusinessClassCapacity,
    int FirstClassCapacity
);