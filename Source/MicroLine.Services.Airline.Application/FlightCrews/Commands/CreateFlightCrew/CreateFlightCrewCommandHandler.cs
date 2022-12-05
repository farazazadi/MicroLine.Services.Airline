using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Application.FlightCrews.Commands.CreateFlightCrew;

public class CreateFlightCrewCommandHandler : IRequestHandler<CreateFlightCrewCommand, FlightCrewDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public CreateFlightCrewCommandHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper
        )
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }

    public async Task<FlightCrewDto> Handle(CreateFlightCrewCommand command, CancellationToken token)
    {
        var validationResult =
            await CheckPassportNumberUniqueness(command.PassportNumber, token)
            + await CheckNationalIdUniqueness(command.NationalId, token);


        if (!validationResult.IsSuccess)
            throw new CreateFlightCrewException(validationResult.GetFailureReasons());



        var fullName = FullName.Create(command.FullName.FirstName, command.FullName.LastName);

        var address = Address.Create(
            command.Address.Street,
            command.Address.City,
            command.Address.State,
            command.Address.Country,
            command.Address.PostalCode
        );

        var flightCrew = FlightCrew.Create(
            command.FlightCrewType,
            command.Gender,
            fullName,
            command.BirthDate,
            command.NationalId,
            command.PassportNumber,
            command.Email,
            command.ContactNumber,
            address
        );

        await _airlineDbContext.FlightCrews.AddAsync(flightCrew, token);

        await _airlineDbContext.SaveChangesAsync(token);

        var flightCrewDto = _mapper.Map<FlightCrewDto>(flightCrew);

        return flightCrewDto;
    }

    private async Task<Result> CheckPassportNumberUniqueness(PassportNumber passportNumber, CancellationToken token)
    {
        var passportNumberExist = await _airlineDbContext.FlightCrews
            .AnyAsync(fc => fc.PassportNumber == passportNumber, token);


        return passportNumberExist
            ? Result.Fail($"A flight crew member with same PassportNumber ({passportNumber}) already exist!")
            : new Result();
    }

    private async Task<Result> CheckNationalIdUniqueness(NationalId nationalId, CancellationToken token)
    {
        var nationalIdExist = await _airlineDbContext.FlightCrews
            .AnyAsync(fc => fc.NationalId == nationalId, token);


        return nationalIdExist
            ? Result.Fail($"A flight crew member with same NationalId ({nationalId}) already exist!")
            : new Result();
    }
}