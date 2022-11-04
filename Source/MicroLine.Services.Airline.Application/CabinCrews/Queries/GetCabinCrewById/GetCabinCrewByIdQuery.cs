using MediatR;
using MicroLine.Services.Airline.Application.CabinCrews.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.CabinCrews.Queries.GetCabinCrewById;

public record GetCabinCrewByIdQuery (Guid Id) : IRequest<CabinCrewDto>;