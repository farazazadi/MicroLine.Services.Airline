using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Aircrafts;

namespace MicroLine.Services.Airline.Application.Aircrafts.Commands.CreateAircraft;

internal class CreateAircraftCommandHandler : IRequestHandler<CreateAircraftCommand, AircraftDto>
{
    private readonly IAirlineDbContext _dbContext;
    private readonly IAircraftReadonlyRepository _aircraftReadonlyRepository;
    private readonly IMapper _mapper;

    public CreateAircraftCommandHandler(
        IAirlineDbContext dbContext,
        IAircraftReadonlyRepository aircraftReadonlyRepository,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _aircraftReadonlyRepository = aircraftReadonlyRepository;
        _mapper = mapper;
    }

    public async Task<AircraftDto> Handle(CreateAircraftCommand command, CancellationToken token)
    {
        var passengerSeatingCapacity = PassengerSeatingCapacity.Create(
            command.PassengerSeatingCapacity.EconomyClassCapacity,
            command.PassengerSeatingCapacity.BusinessClassCapacity,
            command.PassengerSeatingCapacity.FirstClassCapacity
        );

        var aircraft = await Aircraft.CreateAsync(
            command.Manufacturer,
            command.Model,
            command.ManufactureDate,
            passengerSeatingCapacity,
            command.CruisingSpeed,
            command.MaximumOperatingSpeed,
            command.RegistrationCode,

            _aircraftReadonlyRepository,
            token
        );

        await _dbContext.Aircrafts.AddAsync(aircraft, token);

        await _dbContext.SaveChangesAsync(token);

        var aircraftDto = _mapper.Map<AircraftDto>(aircraft);

        return aircraftDto;
    }
}