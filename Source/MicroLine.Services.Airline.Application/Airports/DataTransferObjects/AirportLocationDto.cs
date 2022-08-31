using MicroLine.Services.Airline.Domain.Airports;

namespace MicroLine.Services.Airline.Application.Airports.DataTransferObjects;

public record AirportLocationDto(
    Continent Continent,
    string Country,
    string Region,
    string City,
    double Latitude,
    double Longitude
);