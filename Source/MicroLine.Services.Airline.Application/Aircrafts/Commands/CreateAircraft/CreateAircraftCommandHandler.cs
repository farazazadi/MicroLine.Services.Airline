using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Aircrafts.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Application.Aircrafts.Commands.CreateAircraft;

internal class CreateAircraftCommandHandler : IRequestHandler<CreateAircraftCommand, AircraftDto>
{
    private readonly IAirlineDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateAircraftCommandHandler(
        IAirlineDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<AircraftDto> Handle(CreateAircraftCommand command, CancellationToken token)
    {
        AircraftRegistrationCode registrationCode = command.RegistrationCode;

        var aircraftWithSameRegistrationCodeExist = await _dbContext.Aircrafts.AnyAsync(
            aircraft => aircraft.RegistrationCode == registrationCode, token); ;

        if (aircraftWithSameRegistrationCodeExist)
            throw new DuplicateAircraftRegistrationCodeException(registrationCode);



        var passengerSeatingCapacity = PassengerSeatingCapacity.Create(
            command.PassengerSeatingCapacity.EconomyClassCapacity,
            command.PassengerSeatingCapacity.BusinessClassCapacity,
            command.PassengerSeatingCapacity.FirstClassCapacity
        );

        var aircraft = Aircraft.Create(
            command.Manufacturer,
            command.Model,
            command.ManufactureDate,
            passengerSeatingCapacity,
            command.CruisingSpeed,
            command.MaximumOperatingSpeed,
            registrationCode
        );

        await _dbContext.Aircrafts.AddAsync(aircraft, token);

        await _dbContext.SaveChangesAsync(token);

        var aircraftDto = _mapper.Map<AircraftDto>(aircraft);

        return aircraftDto;
    }
}