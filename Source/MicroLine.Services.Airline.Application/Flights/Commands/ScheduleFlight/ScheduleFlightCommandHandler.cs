using MapsterMapper;
using MediatR;
using MicroLine.Services.Airline.Application.Common.Contracts;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Domain.FlightPricingPolicies;
using MicroLine.Services.Airline.Domain.Flights;
using Microsoft.EntityFrameworkCore;

namespace MicroLine.Services.Airline.Application.Flights.Commands.ScheduleFlight;

internal class ScheduleFlightCommandHandler : IRequestHandler<ScheduleFlightCommand, FlightDto>
{
    private readonly IAirlineDbContext _airlineDbContext;
    private readonly IEnumerable<IFlightPricingPolicy> _flightPricingPolicies;
    private readonly IMapper _mapper;

    public ScheduleFlightCommandHandler(
        IAirlineDbContext airlineDbContext,
        IEnumerable<IFlightPricingPolicy> flightPricingPolicies,
        IMapper mapper)
    {
        _airlineDbContext = airlineDbContext;
        _flightPricingPolicies = flightPricingPolicies;

        _mapper = mapper;
    }

    public async Task<FlightDto> Handle(ScheduleFlightCommand command, CancellationToken token)
    {
        Result validationResult = new();


        var originAirport = await GetOriginAirportIfExistAsync(command, validationResult, token);

        var destinationAirport = await GetDestinationAirportIfExistAsync(command, validationResult, token);

        var aircraft = await GetAircraftIfExistAsync(command, validationResult, token);

        var flightCrewMembers = await GetFlightCrewMembersIfExistAsync(command, validationResult, token);

        var cabinCrewMembers = await GetCabinCrewMembersIfExistAsync(command, validationResult, token);


        if (!validationResult.IsSuccess)
            throw new ScheduleFlightException(validationResult.GetFailureReasons());



        var flightBasePrice = FlightPrice.Create(
            Money.Of(command.BasePrices.EconomyClass.Amount, command.BasePrices.EconomyClass.Currency),
            Money.Of(command.BasePrices.BusinessClass.Amount, command.BasePrices.BusinessClass.Currency),
            Money.Of(command.BasePrices.FirstClass.Amount, command.BasePrices.FirstClass.Currency)
        );


        var flight = Flight.ScheduleNewFlight(
            _flightPricingPolicies,
            FlightNumber.Create(command.FlightNumber),
            originAirport,
            destinationAirport,
            aircraft,
            command.ScheduledUtcDateTimeOfDeparture,
            flightBasePrice,
            flightCrewMembers,
            cabinCrewMembers
        );


        validationResult =
              await CheckAircraftAvailabilityAsync(flight, token)
            + await CheckFlightCrewMembersAvailabilityAsync(flight, token)
            + await CheckCabinCrewMembersAvailabilityAsync(flight, token);


        if (!validationResult.IsSuccess)
            throw new ScheduleFlightException(validationResult.GetFailureReasons());


        await _airlineDbContext.Flights.AddAsync(flight, token);
        await _airlineDbContext.SaveChangesAsync(token);

        var flightDto = _mapper.Map<FlightDto>(flight);

        return flightDto;
    }


    private async Task<Airport> GetOriginAirportIfExistAsync(
        ScheduleFlightCommand command,
        Result validationResult,
        CancellationToken token)
    {
        var originAirportId = command.OriginAirportId;

        var airport = await GetAirportAsync(originAirportId, token);

        if (airport is null)
            validationResult.WithFailure($"Origin airport with Id ({originAirportId}) can not be found!");

        return airport;
    }

    private async Task<Airport> GetDestinationAirportIfExistAsync(
        ScheduleFlightCommand command,
        Result validationResult,
        CancellationToken token)
    {
        var destinationAirportId = command.DestinationAirportId;

        var airport = await GetAirportAsync(destinationAirportId, token);

        if (airport is null)
            validationResult.WithFailure($"Destination airport with Id ({destinationAirportId}) can not be found!");

        return airport;
    }

    private async Task<Airport> GetAirportAsync(Id airportId, CancellationToken token)
    {
        return await _airlineDbContext.Airports.FindAsync(new object[] { airportId }, token);
    }


    private async Task<Aircraft> GetAircraftIfExistAsync(
        ScheduleFlightCommand command,
        Result validationResult,
        CancellationToken token)
    {
        Id aircraftId = command.AircraftId;

        var aircraft = await _airlineDbContext.Aircrafts.FindAsync(new object[] { aircraftId }, token);

        if (aircraft is null)
            validationResult.WithFailure($"Aircraft with Id ({aircraftId}) can not be found!");

        return aircraft;
    }


    private async Task<List<FlightCrew>> GetFlightCrewMembersIfExistAsync(
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


    private async Task<List<CabinCrew>> GetCabinCrewMembersIfExistAsync(
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


    
    private async Task<Result> CheckAircraftAvailabilityAsync(Flight flight, CancellationToken token)
    {
        var overlappingFlight = await _airlineDbContext.Flights
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Aircraft == flight.Aircraft
                                      && f.ScheduledUtcDateTimeOfDeparture < flight.ScheduledUtcDateTimeOfArrival
                                      && flight.ScheduledUtcDateTimeOfDeparture < f.ScheduledUtcDateTimeOfArrival
                , token);

        return overlappingFlight is not null ?
            Result.Fail($"The aircraft ({flight.Aircraft.Id}) is unavailable due to an overlap with the flight ({overlappingFlight.Id})!")
            : new Result();
    }


    private async Task<Result> CheckFlightCrewMembersAvailabilityAsync(Flight flight, CancellationToken token)
    {
        Result result = new();

        var overlappingFlights = await _airlineDbContext.Flights
            .AsNoTracking()
            .Include(f => f.FlightCrewMembers)
            .Where(f => f.FlightCrewMembers.Any(fc => flight.FlightCrewMembers.Contains(fc))
                              && f.ScheduledUtcDateTimeOfDeparture < flight.ScheduledUtcDateTimeOfArrival
                              && flight.ScheduledUtcDateTimeOfDeparture < f.ScheduledUtcDateTimeOfArrival
                ).ToListAsync(token);


        if (overlappingFlights.Count == 0) return result;


        foreach (var overlappingFlight in overlappingFlights)
        {
            flight.FlightCrewMembers
                .Intersect(overlappingFlight.FlightCrewMembers)
                .ToList()
                .ForEach(flightCrew =>
                    result.WithFailure($"The Flight Crew ({flightCrew.Id}) is unavailable due to an overlap with the flight ({overlappingFlight.Id})!")
                );
        }

        return result;
    }


    private async Task<Result> CheckCabinCrewMembersAvailabilityAsync(Flight flight, CancellationToken token)
    {
        Result result = new();

        var overlappingFlights = await _airlineDbContext.Flights
            .AsNoTracking()
            .Include(f=> f.CabinCrewMembers)
            .Where(f => f.CabinCrewMembers.Any(cc => flight.CabinCrewMembers.Contains(cc))
                        && f.ScheduledUtcDateTimeOfDeparture < flight.ScheduledUtcDateTimeOfArrival
                        && flight.ScheduledUtcDateTimeOfDeparture < f.ScheduledUtcDateTimeOfArrival
            ).ToListAsync(token);


        if (overlappingFlights.Count == 0) return result;


        foreach (var overlappingFlight in overlappingFlights)
        {
            flight.CabinCrewMembers
                .Intersect(overlappingFlight.CabinCrewMembers)
                .ToList()
                .ForEach(cabinCrew =>
                    result.WithFailure($"The Cabin Crew ({cabinCrew.Id}) is unavailable due to an overlap with the flight ({overlappingFlight.Id})!")
                );
        }

        return result;
    }

}