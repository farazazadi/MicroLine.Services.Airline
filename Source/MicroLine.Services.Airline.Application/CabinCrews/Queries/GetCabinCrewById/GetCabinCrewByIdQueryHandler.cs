using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.CabinCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.CabinCrews;

namespace MicroLine.Services.Airline.Application.CabinCrews.Queries.GetCabinCrewById;

internal class GetCabinCrewByIdQueryHandler : IRequestHandler<GetCabinCrewByIdQuery, CabinCrewDto>
{
    private readonly ICabinCrewReadonlyRepository _cabinCrewReadonlyRepository;
    private readonly IMapper _mapper;

    public GetCabinCrewByIdQueryHandler(
        ICabinCrewReadonlyRepository cabinCrewReadonlyRepository,
        IMapper mapper
        )
    {
        _cabinCrewReadonlyRepository = cabinCrewReadonlyRepository;
        _mapper = mapper;
    }

    public async Task<CabinCrewDto> Handle(GetCabinCrewByIdQuery query, CancellationToken token)
    {
        var cabinCrew = await _cabinCrewReadonlyRepository.GetAsync(query.Id, token);

        var cabinCrewDto = _mapper.Map<CabinCrewDto>(cabinCrew);

        return cabinCrewDto;
    }
}