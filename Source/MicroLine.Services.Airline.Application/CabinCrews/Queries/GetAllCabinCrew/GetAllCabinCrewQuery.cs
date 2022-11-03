using MediatR;
using MicroLine.Services.Airline.Application.CabinCrews.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.CabinCrews.Queries.GetAllCabinCrew;

public record GetAllCabinCrewQuery : IRequest<IReadOnlyList<CabinCrewDto>>;