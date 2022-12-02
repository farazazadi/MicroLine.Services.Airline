using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;
using MicroLine.Services.Airline.Application.Flights.Exceptions;
using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Domain.FlightPricingPolicies;
using MicroLine.Services.Airline.Domain.Flights;
using Microsoft.EntityFrameworkCore;
using Result = MicroLine.Services.Airline.Domain.Common.Result;

namespace MicroLine.Services.Airline.Application.Flights.Commands.ScheduleFlight;

internal class ScheduleFlightCommandHandler : IRequestHandler<ScheduleFlightCommand, FlightDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IFlightReadonlyRepository _flightReadonlyRepository;
    private readonly IEnumerable<IFlightPricingPolicy> _flightPricingPolicies;
    private readonly IMapper _mapper;

    public ScheduleFlightCommandHandler(
        IAirlineDbContext airlineDbContext,
        IFlightReadonlyRepository flightReadonlyRepository,
        IEnumerable<IFlightPricingPolicy> flightPricingPolicies,
        IMapper mapper)
    {
        _airlineDbContext = airlineDbContext;
        _flightReadonlyRepository = flightReadonlyRepository;
        _flightPricingPolicies = flightPricingPolicies;

        _mapper = mapper;
    }

    public async Task<FlightDto> Handle(ScheduleFlightCommand command, CancellationToken token)
    {
        var validationResult = new Result();


        var originAirport = await GetOriginAirportAsync(command, validationResult, token);

        var destinationAirport = await GetDestinationAirportAsync(command, validationResult, token);

        var aircraft = await GetAircraftAsync(command, validationResult, token);

        var flightCrewMembers = await GetFlightCrewMembersAsync(command, validationResult, token);

        var cabinCrewMembers = await GetCabinCrewMembersAsync(command, validationResult, token);



        if (!validationResult.IsSuccess)
            throw new ScheduleFlightException(string.Join(Environment.NewLine, validationResult.FailureReasons));



        var flightBasePrice = FlightPrice.Create(
            Money.Of(command.BasePrices.EconomyClass.Amount, command.BasePrices.EconomyClass.Currency),
            Money.Of(command.BasePrices.BusinessClass.Amount, command.BasePrices.BusinessClass.Currency),
            Money.Of(command.BasePrices.FirstClass.Amount, command.BasePrices.FirstClass.Currency)
        );


        var flight = await Flight.ScheduleNewFlightAsync(
            _flightReadonlyRepository,
            _flightPricingPolicies,
            FlightNumber.Create(command.FlightNumber),
            originAirport,
            destinationAirport,
            aircraft,
            command.ScheduledUtcDateTimeOfDeparture,
            flightBasePrice,
            flightCrewMembers,
            cabinCrewMembers,
            token
        );

        var flightDto = _mapper.Map<FlightDto>(flight);

        return flightDto;
    }


    private async Task<Airport> GetOriginAirportAsync(
        ScheduleFlightCommand command,
        Result validationResult,
        CancellationToken token = default)
    {
        var originAirportId = command.OriginAirportId;

        var airport = await GetAirportAsync(token, originAirportId);

        if (airport is null)
            validationResult.WithFailure($"Origin airport with Id ({originAirportId}) can not be found!");

        return airport;
    }

    private async Task<Airport> GetDestinationAirportAsync(
        ScheduleFlightCommand command,
        Result validationResult,
        CancellationToken token = default)
    {
        var destinationAirportId = command.DestinationAirportId;

        var airport = await GetAirportAsync(token, destinationAirportId);

        if (airport is null)
            validationResult.WithFailure($"Origin airport with Id ({destinationAirportId}) can not be found!");

        return airport;
    }

    private async Task<Airport> GetAirportAsync(CancellationToken token, Id airportId)
    {
        return await _airlineDbContext.Airports.FindAsync(new object[] { airportId }, token);
    }


    private async Task<Aircraft> GetAircraftAsync(
        ScheduleFlightCommand command,
        Result validationResult,
        CancellationToken token = default)
    {
        Id aircraftId = command.AircraftId;

        var aircraft = await _airlineDbContext.Aircrafts.FindAsync(new object[] { aircraftId }, token);

        if (aircraft is null)
            validationResult.WithFailure($"Aircraft with Id ({aircraftId}) can not be found!");

        return aircraft;
    }


    private async Task<List<FlightCrew>> GetFlightCrewMembersAsync(
        ScheduleFlightCommand command,
        Result validationResult,
        CancellationToken token = default)
    {

        var flightCrewMembersIdList = command.FlightCrewMembers;

        var flightCrewMembers = await _airlineDbContext.FlightCrews
            .Where(fc => flightCrewMembersIdList.Contains(fc.Id))
            .ToListAsync(token);

        var invalidFlightCrewMembersIdList = flightCrewMembersIdList
            .Except(flightCrewMembers.Select(fc => (Guid)fc.Id))
            .ToList();

        foreach (var id in invalidFlightCrewMembersIdList)
            validationResult.WithFailure($"Flight Crew with Id ({id}) can not be found!");


        return flightCrewMembers;
    }


    private async Task<List<CabinCrew>> GetCabinCrewMembersAsync(
        ScheduleFlightCommand command,
        Result validationResult,
        CancellationToken token = default)
    {

        var cabinCrewMembersIdList = command.CabinCrewMembers;

        var cabinCrewMembers = await _airlineDbContext.CabinCrews
            .Where(cc => cabinCrewMembersIdList.Contains(cc.Id))
            .ToListAsync(token);

        var invalidCabinCrewMembersIdList = cabinCrewMembersIdList
            .Except(cabinCrewMembers.Select(fc => (Guid)fc.Id))
            .ToList();

        foreach (var id in invalidCabinCrewMembersIdList)
            validationResult.WithFailure($"Cabin Crew with Id ({id}) can not be found!");


        return cabinCrewMembers;
    }

}