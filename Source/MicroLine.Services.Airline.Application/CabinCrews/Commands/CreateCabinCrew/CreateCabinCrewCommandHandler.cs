using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.CabinCrews.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Application.CabinCrews.Commands.CreateCabinCrew;

internal class CreateCabinCrewCommandHandler : IRequestHandler<CreateCabinCrewCommand, CabinCrewDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IMapper _mapper;

    public CreateCabinCrewCommandHandler(
        IAirlineDbContext airlineDbContext,
        IMapper mapper)
    {
        _airlineDbContext = airlineDbContext;
        _mapper = mapper;
    }

    public async Task<CabinCrewDto> Handle(CreateCabinCrewCommand command, CancellationToken token)
    {
        var validationResult =
            await CheckPassportNumberUniqueness(command.PassportNumber, token)
            + await CheckNationalIdUniqueness(command.NationalId, token);

        if (!validationResult.IsSuccess)
            throw new CreateCabinCrewException(validationResult.GetFailureReasons());


        var fullName = FullName.Create(command.FullName.FirstName, command.FullName.LastName);

        var address = Address.Create(
            command.Address.Street,
            command.Address.City,
            command.Address.State,
            command.Address.Country,
            command.Address.PostalCode
            );

        var cabinCrew = CabinCrew.Create(
            command.CabinCrewType,
            command.Gender,
            fullName,
            command.BirthDate,
            command.NationalId,
            command.PassportNumber,
            command.Email,
            command.ContactNumber,
            address
        );

        await _airlineDbContext.CabinCrews.AddAsync(cabinCrew, token);

        await _airlineDbContext.SaveChangesAsync(token);

        var cabinCrewDto = _mapper.Map<CabinCrewDto>(cabinCrew);

        return cabinCrewDto;
    }


    private async Task<Result> CheckPassportNumberUniqueness(PassportNumber passportNumber, CancellationToken token)
    {
        var passportNumberExist = await _airlineDbContext.CabinCrews
            .AnyAsync(cc => cc.PassportNumber == passportNumber, token);


        return passportNumberExist
            ? Result.Fail($"A Cabin crew member with same PassportNumber ({passportNumber}) already exist!")
            : new Result();
    }

    private async Task<Result> CheckNationalIdUniqueness(NationalId nationalId, CancellationToken token)
    {
        var nationalIdExist = await _airlineDbContext.CabinCrews
            .AnyAsync(cc => cc.NationalId == nationalId, token);


        return nationalIdExist
            ? Result.Fail($"A Cabin crew member with same NationalId ({nationalId}) already exist!")
            : new Result();
    }
}