using MediatR;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.Airports.Queries.GetAllAirports;

public record GetAllAirportsQuery : IRequest<IReadOnlyList<AirportDto>>;