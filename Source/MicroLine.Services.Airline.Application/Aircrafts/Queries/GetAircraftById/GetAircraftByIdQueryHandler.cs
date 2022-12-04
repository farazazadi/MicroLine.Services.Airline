using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Application.Aircrafts.Queries.GetAircraftById;

internal class GetAircraftByIdQueryHandler : IRequestHandler<GetAircraftByIdQuery, AircraftDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public GetAircraftByIdQueryHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper
        )
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }


    public async Task<AircraftDto> Handle(GetAircraftByIdQuery query, CancellationToken token)
    {
        var aircraft = await _airlineDbContext.Aircrafts.FindAsync(new object[] { (Id)query.Id }, token);

        var aircraftDto = _mapper.Map<AircraftDto>(aircraft);

        return aircraftDto;
    }
}