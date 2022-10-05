using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;

namespace MicroLine.Services.Airline.Application.FlightCrews.Commands.CreateFlightCrew;

public class CreateFlightCrewCommandHandler : IRequestHandler<CreateFlightCrewCommand, FlightCrewDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IFlightCrewReadonlyRepository _flightCrewReadonlyRepository;
    private readonly IMapper _mapper;

    public CreateFlightCrewCommandHandler(
        IAirlineDbContext airlineDbContext,
        IFlightCrewReadonlyRepository flightCrewReadonlyRepository,
        IMapper mapper
        )
    {
        _airlineDbContext = airlineDbContext;
        _flightCrewReadonlyRepository = flightCrewReadonlyRepository;
        _mapper = mapper;
    }
    public async Task<FlightCrewDto> Handle(CreateFlightCrewCommand command, CancellationToken token)
    {
        var fullName = FullName.Create(command.FullName.FirstName, command.FullName.LastName);

        var address = Address.Create(
            command.Address.Street,
            command.Address.City,
            command.Address.State,
            command.Address.Country,
            command.Address.PostalCode
        );

        var flightCrew = await FlightCrew.CreateAsync(
            command.FlightCrewType,
            command.Gender,
            fullName,
            command.BirthDate,
            command.NationalId,
            command.PassportNumber,
            command.Email,
            command.ContactNumber,
            address,

            _flightCrewReadonlyRepository,
            token
        );

        await _airlineDbContext.FlightCrews.AddAsync(flightCrew, token);

        await _airlineDbContext.SaveChangesAsync(token);

        var flightCrewDto = _mapper.Map<FlightCrewDto>(flightCrew);

        return flightCrewDto;
    }
}