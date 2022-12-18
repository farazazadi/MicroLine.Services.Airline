namespace MicroLine.Services.Airline.Infrastructure.Integration.Aircrews;

internal record AircrewIntegrationDto(
    string Id,
    string FullName,
    string Email,
    string ContactNumber
);