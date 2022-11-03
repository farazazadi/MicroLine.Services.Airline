using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.CabinCrews.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Application.CabinCrews.Commands.CreateCabinCrew;

internal class CreateCabinCrewCommandHandler : IRequestHandler<CreateCabinCrewCommand, CabinCrewDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly ICabinCrewReadonlyRepository _cabinCrewReadonlyRepository;
    private readonly IMapper _mapper;

    public CreateCabinCrewCommandHandler(
        IAirlineDbContext airlineDbContext,
        ICabinCrewReadonlyRepository cabinCrewReadonlyRepository,
        IMapper mapper)
    {
        _airlineDbContext = airlineDbContext;
        _cabinCrewReadonlyRepository = cabinCrewReadonlyRepository;
        _mapper = mapper;
    }

    public async Task<CabinCrewDto> Handle(CreateCabinCrewCommand command, CancellationToken token)
    {
        var fullName = FullName.Create(command.FullName.FirstName, command.FullName.LastName);

        var address = Address.Create(
            command.Address.Street,
            command.Address.City,
            command.Address.State,
            command.Address.Country,
            command.Address.PostalCode
            );

        var cabinCrew = await CabinCrew.CreateAsync(
            command.CabinCrewType,
            command.Gender,
            fullName,
            command.BirthDate,
            command.NationalId,
            command.PassportNumber,
            command.Email,
            command.ContactNumber,
            address,

            _cabinCrewReadonlyRepository,
            token
        );

        await _airlineDbContext.CabinCrews.AddAsync(cabinCrew, token);

        await _airlineDbContext.SaveChangesAsync(token);

        var cabinCrewDto = _mapper.Map<CabinCrewDto>(cabinCrew);

        return cabinCrewDto;
    }
}