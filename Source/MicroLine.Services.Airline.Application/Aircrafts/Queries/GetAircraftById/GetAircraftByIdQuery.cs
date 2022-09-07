using MediatR;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.Aircrafts.Queries.GetAircraftById;

public record GetAircraftByIdQuery(Guid Id) : IRequest<AircraftDto>;