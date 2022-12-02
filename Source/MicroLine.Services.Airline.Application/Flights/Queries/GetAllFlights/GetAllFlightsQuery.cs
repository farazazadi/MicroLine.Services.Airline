using MediatR;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.Flights.Queries.GetAllFlights;

public record GetAllFlightsQuery : IRequest<IReadOnlyList<FlightDto>>;