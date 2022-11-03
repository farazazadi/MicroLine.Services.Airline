using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.CabinCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.CabinCrews;

namespace MicroLine.Services.Airline.Application.CabinCrews.Queries.GetAllCabinCrew;

internal class GetAllCabinCrewQueryHandler : IRequestHandler<GetAllCabinCrewQuery, IReadOnlyList<CabinCrewDto>>
{
    private readonly ICabinCrewReadonlyRepository _cabinCrewReadonlyRepository;
    private readonly IMapper _mapper;

    public GetAllCabinCrewQueryHandler(
        ICabinCrewReadonlyRepository cabinCrewReadonlyRepository,
        IMapper mapper
        )
    {
        _cabinCrewReadonlyRepository = cabinCrewReadonlyRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CabinCrewDto>> Handle(GetAllCabinCrewQuery query, CancellationToken token)
    {
        var cabinCrewList = await _cabinCrewReadonlyRepository.GetAllAsync(token);

        var cabinCrewDtoList = _mapper.Map<IReadOnlyList<CabinCrewDto>>(cabinCrewList);

        return cabinCrewDtoList;
    }
}