using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.FlightCrew;

public class FlightCrew : AggregateRoot
{

    public FlightCrewType FlightCrewType { get; }
    public Gender Gender { get; }
    public FullName FullName { get; }
    public Date BirtDate { get; }
    public NationalId NationalId { get; }
    public PassportNumber PassportNumber { get; }
    public Email Email { get; }
    public ContactNumber ContactNumber { get; }
    public Address Address { get; }


    private FlightCrew(FlightCrewType flightCrewType, Gender gender, FullName fullName,
                                    Date birtDate, NationalId nationalId, PassportNumber passportNumber,
                                    Email email, ContactNumber contactNumber, Address address)
    {
        FlightCrewType = flightCrewType;
        Gender = gender;
        FullName = fullName;
        BirtDate = birtDate;
        NationalId = nationalId;
        PassportNumber = passportNumber;
        Email = email;
        ContactNumber = contactNumber;
        Address = address;
    }


    public static FlightCrew Create(FlightCrewType flightCrewType, Gender gender, FullName fullName,
                                    Date birtDate, NationalId nationalId, PassportNumber passportNumber,
                                    Email email, ContactNumber contactNumber, Address address)
    {

        return new FlightCrew(flightCrewType, gender, fullName, birtDate,
                            nationalId, passportNumber, email, contactNumber, address);
    }
}