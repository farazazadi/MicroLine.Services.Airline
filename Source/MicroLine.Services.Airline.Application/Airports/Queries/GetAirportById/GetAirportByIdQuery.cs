using MediatR;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.Airports.Queries.GetAirportById;

public record GetAirportByIdQuery(Guid Id) : IRequest<AirportDto>;