using MediatR;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.Airports.Commands.CreateAirport;

public record CreateAirportCommand(
    string IcaoCode,
    string IataCode,
    string Name,
    BaseUtcOffsetDto BaseUtcOffsetDto,
    AirportLocationDto AirportLocationDto

) : IRequest<AirportDto>;