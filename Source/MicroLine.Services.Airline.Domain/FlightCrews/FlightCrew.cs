using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.FlightCrews;

public class FlightCrew : AggregateRoot
{

    public FlightCrewType FlightCrewType { get; }
    public Gender Gender { get; }
    public FullName FullName { get; }
    public Date BirthDate { get; }
    public NationalId NationalId { get; }
    public PassportNumber PassportNumber { get; }
    public Email Email { get; }
    public ContactNumber ContactNumber { get; }
    public Address Address { get; }


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