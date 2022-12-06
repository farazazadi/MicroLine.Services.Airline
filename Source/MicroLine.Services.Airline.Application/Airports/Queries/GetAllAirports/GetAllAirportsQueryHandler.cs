using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Application.Airports.Queries.GetAllAirports;

public class GetAllAirportsQueryHandler : IRequestHandler<GetAllAirportsQuery, IReadOnlyList<AirportDto>>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public GetAllAirportsQueryHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper
        )
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AirportDto>> Handle(GetAllAirportsQuery query, CancellationToken token)
    {
        var airports = await _airlineDbContext.Airports
            .AsNoTracking()
            .ToListAsync(token);

        var airportDtoList = _mapper.Map<List<AirportDto>>(airports);

        return airportDtoList;
    }
}