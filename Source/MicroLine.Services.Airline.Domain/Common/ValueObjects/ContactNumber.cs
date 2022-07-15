using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.Extensions;
using System.Text.RegularExpressions;

namespace MicroLine.Services.Airline.Domain.Common.ValueObjects;

public sealed class ContactNumber : ValueObject
{
    private readonly string _contactNumber;

    private ContactNumber(string contactNumber)
    {
        _contactNumber = contactNumber;
    }

    public static ContactNumber Create(string contactNumber)
    {
        Validate(contactNumber);

        return new ContactNumber(contactNumber.Trim());
    }

    private static void Validate(string contactNumber)
    {
        if (contactNumber.IsNullOrEmpty())
            throw new InvalidContactNumberException("Contact number can not be null or empty!");

        var pattern = @"^(?:00|\+)(?!0)\d{10,15}$";

        if (!Regex.IsMatch(contactNumber, pattern, RegexOptions.Compiled))
            throw new InvalidContactNumberException($"Contact number is not in the correct format!");
    }


    public static implicit operator ContactNumber(string contactNumber) => Create(contactNumber);
    public static implicit operator string(ContactNumber contactNumber) => contactNumber.ToString();

    public override string ToString() => _contactNumber;
}