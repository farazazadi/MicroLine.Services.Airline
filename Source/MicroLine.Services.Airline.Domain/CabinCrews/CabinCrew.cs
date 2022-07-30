using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.CabinCrews;

public class CabinCrew : AggregateRoot
{
    public CabinCrewType CabinCrewType { get; }
    public Gender Gender { get; }
    public FullName FullName { get; }
    public Date BirtDate { get; }
    public NationalId NationalId { get; }
    public PassportNumber PassportNumber { get; }
    public Email Email { get; }
    public ContactNumber ContactNumber { get; }
    public Address Address { get; }

    private CabinCrew(CabinCrewType cabinCrewType, Gender gender, FullName fullName, Date birtDate,
                      NationalId nationalId, PassportNumber passportNumber,
                      Email email, ContactNumber contactNumber, Address address)
    {
        CabinCrewType = cabinCrewType;
        Gender = gender;
        FullName = fullName;
        BirtDate = birtDate;
        NationalId = nationalId;
        PassportNumber = passportNumber;
        Email = email;
        ContactNumber = contactNumber;
        Address = address;
    }



    public static CabinCrew Create(CabinCrewType cabinCrewType, Gender gender, FullName fullName, Date birtDate,
                        NationalId nationalId, PassportNumber passportNumber,
                        Email email, ContactNumber contactNumber, Address address)
    {
        return new CabinCrew(cabinCrewType, gender, fullName, birtDate, nationalId,
                             passportNumber, email, contactNumber, address);
    }

}