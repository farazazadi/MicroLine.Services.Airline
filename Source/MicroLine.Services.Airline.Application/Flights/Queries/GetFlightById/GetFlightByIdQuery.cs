using MediatR;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.Flights.Queries.GetFlightById;

public record GetFlightByIdQuery(Guid Id) : IRequest<FlightDto>;