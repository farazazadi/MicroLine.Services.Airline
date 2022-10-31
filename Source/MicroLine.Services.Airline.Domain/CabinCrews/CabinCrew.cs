using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.Flights;

namespace MicroLine.Services.Airline.Domain.CabinCrews;

public class CabinCrew : AggregateRoot
{
    public CabinCrewType CabinCrewType { get; private set; }
    public Gender Gender { get; private set; }
    public FullName FullName { get; private set; }
    public Date BirthDate { get; private set; }
    public NationalId NationalId { get; private set; }
    public PassportNumber PassportNumber { get; private set; }
    public Email Email { get; private set; }
    public ContactNumber ContactNumber { get; private set; }
    public Address Address { get; private set; }

    public IReadOnlyList<Flight> Flights { get; private set; } = new List<Flight>();

    private CabinCrew() { }

    private CabinCrew(CabinCrewType cabinCrewType, Gender gender, FullName fullName, Date birthDate,
        NationalId nationalId, PassportNumber passportNumber,
        Email email, ContactNumber contactNumber, Address address)
    {
        CabinCrewType = cabinCrewType;
        Gender = gender;
        FullName = fullName;
        BirthDate = birthDate;
        NationalId = nationalId;
        PassportNumber = passportNumber;
        Email = email;
        ContactNumber = contactNumber;
        Address = address;
    }



    public static async Task<CabinCrew> CreateAsync(CabinCrewType cabinCrewType, Gender gender, FullName fullName, Date birthDate,
        NationalId nationalId, PassportNumber passportNumber,
        Email email, ContactNumber contactNumber, Address address,
        ICabinCrewReadonlyRepository cabinCrewReadonlyRepository,
        CancellationToken token = default)
    {

        var passportNumberExist = await cabinCrewReadonlyRepository.ExistAsync(passportNumber, token);

        if (passportNumberExist)
            throw new DuplicatePassportNumberException(
                $"A cabin crew member with same PassportNumber ({passportNumber}) already exist!");


        var nationalIdExist = await cabinCrewReadonlyRepository.ExistAsync(nationalId, token);

        if (nationalIdExist)
            throw new DuplicateNationalIdException(
                $"A cabin crew member with same NationalId ({nationalId}) already exist!");


        return new CabinCrew(cabinCrewType, gender, fullName, birthDate, nationalId,
            passportNumber, email, contactNumber, address);
    }

}