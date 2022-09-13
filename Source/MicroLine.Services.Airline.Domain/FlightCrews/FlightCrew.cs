using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.Flights;

namespace MicroLine.Services.Airline.Domain.FlightCrews;

public class FlightCrew : AggregateRoot
{

    public FlightCrewType FlightCrewType { get; private set; }
    public Gender Gender { get; private set; }
    public FullName FullName { get; private set; }
    public Date BirthDate { get; private set; }
    public NationalId NationalId { get; private set; }
    public PassportNumber PassportNumber { get; private set; }
    public Email Email { get; private set; }
    public ContactNumber ContactNumber { get; private set; }
    public Address Address { get; private set; }

    public IReadOnlyList<Flight> Flights { get; private set; } = new List<Flight>();

    private FlightCrew() { }

    private FlightCrew(FlightCrewType flightCrewType, Gender gender, FullName fullName,
        Date birthDate, NationalId nationalId, PassportNumber passportNumber,
        Email email, ContactNumber contactNumber, Address address)
    {
        FlightCrewType = flightCrewType;
        Gender = gender;
        FullName = fullName;
        BirthDate = birthDate;
        NationalId = nationalId;
        PassportNumber = passportNumber;
        Email = email;
        ContactNumber = contactNumber;
        Address = address;
    }


    public static async Task<FlightCrew> CreateAsync(FlightCrewType flightCrewType, Gender gender, FullName fullName,
        Date birthDate, NationalId nationalId, PassportNumber passportNumber,
        Email email, ContactNumber contactNumber, Address address,
        IFlightCrewReadonlyRepository flightCrewReadonlyRepository,
        CancellationToken token = default)
    {
        var passportNumberExist = await flightCrewReadonlyRepository.ExistAsync(passportNumber, token);

        if (passportNumberExist)
            throw new DuplicatePassportNumberException(
                $"A flight crew member with same PassportNumber ({passportNumber}) already exist!");


        var nationalIdExist = await flightCrewReadonlyRepository.ExistAsync(nationalId, token);

        if (nationalIdExist)
            throw new DuplicateNationalIdException(
                $"A flight crew member with same NationalId ({nationalId}) already exist!");


        return new FlightCrew(flightCrewType, gender, fullName, birthDate,
            nationalId, passportNumber, email, contactNumber, address);
    }
}