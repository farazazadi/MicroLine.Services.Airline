namespace MicroLine.Services.Airline.Infrastructure.Integration.Aircrafts;

internal record AircraftIntegrationDto(
    string Id,
    string Manufacturer,
    string Model,
    int EconomyClassCapacity,
    int BusinessClassCapacity,
    int FirstClassCapacity
);