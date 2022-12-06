using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.CabinCrews.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Application.CabinCrews.Queries.GetAllCabinCrew;

internal class GetAllCabinCrewQueryHandler : IRequestHandler<GetAllCabinCrewQuery, IReadOnlyList<CabinCrewDto>>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public GetAllCabinCrewQueryHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper
        )
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CabinCrewDto>> Handle(GetAllCabinCrewQuery query, CancellationToken token)
    {
        var cabinCrewList = await _airlineDbContext.CabinCrews
            .AsNoTracking()
            .OrderByDescending(cabinCrew => cabinCrew.CreatedAtUtc)
            .ToListAsync(token);

        var cabinCrewDtoList = _mapper.Map<IReadOnlyList<CabinCrewDto>>(cabinCrewList);

        return cabinCrewDtoList;
    }
}