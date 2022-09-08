using MediatR;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.Aircrafts.Queries.GetAllAircrafts;

public record GetAllAircraftsQuery : IRequest<IReadOnlyList<AircraftDto>>;