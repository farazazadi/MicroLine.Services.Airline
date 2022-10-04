using MediatR;
using MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.FlightCrews.Queries.GetFlightCrewById;

public record GetFlightCrewByIdQuery(Guid Id) : IRequest<FlightCrewDto>;