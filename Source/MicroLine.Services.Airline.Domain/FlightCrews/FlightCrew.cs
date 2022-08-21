using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

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


    public static FlightCrew Create(FlightCrewType flightCrewType, Gender gender, FullName fullName,
        Date birthDate, NationalId nationalId, PassportNumber passportNumber,
        Email email, ContactNumber contactNumber, Address address)
    {

        return new FlightCrew(flightCrewType, gender, fullName, birthDate,
            nationalId, passportNumber, email, contactNumber, address);
    }
}