using MediatR;
using MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.FlightCrews.Queries.GetAllFlightCrew;

public record GetAllFlightCrewQuery : IRequest<IReadOnlyList<FlightCrewDto>>;