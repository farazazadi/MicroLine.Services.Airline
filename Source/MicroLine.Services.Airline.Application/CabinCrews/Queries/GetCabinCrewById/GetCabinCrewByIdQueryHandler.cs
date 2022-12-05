using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.CabinCrews.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Application.CabinCrews.Queries.GetCabinCrewById;

internal class GetCabinCrewByIdQueryHandler : IRequestHandler<GetCabinCrewByIdQuery, CabinCrewDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public GetCabinCrewByIdQueryHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper
        )
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }

    public async Task<CabinCrewDto> Handle(GetCabinCrewByIdQuery query, CancellationToken token)
    {
        var cabinCrew = await _airlineDbContext.CabinCrews
            .FindAsync(new object[] { (Id)query.Id }, token);

        var cabinCrewDto = _mapper.Map<CabinCrewDto>(cabinCrew);

        return cabinCrewDto;
    }
}