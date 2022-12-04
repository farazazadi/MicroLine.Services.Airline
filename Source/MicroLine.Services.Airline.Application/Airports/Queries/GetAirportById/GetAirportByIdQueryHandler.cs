using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Application.Airports.Queries.GetAirportById;

public class GetAirportByIdQueryHandler : IRequestHandler<GetAirportByIdQuery, AirportDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public GetAirportByIdQueryHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper
        )
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }

    public async Task<AirportDto> Handle(GetAirportByIdQuery query, CancellationToken token)
    {
        var airport = await _airlineDbContext.Airports
            .FindAsync(new object[] { (Id)query.Id }, token);

        return airport is null ? null : _mapper.Map<AirportDto>(airport);
    }
}