namespace MicroLine.Services.Airline.Domain.Flights;

public enum FlightStatus
{
    Unknown = 0,
    Scheduled = 1,
    Canceled = 2,
    Departed = 3,
    Landed = 4,
    EmergencyLanding = 5
}