using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.Extensions;

namespace MicroLine.Services.Airline.Domain.Common.ValueObjects;

public sealed class Address : ValueObject
{
    public static Address Unknown => new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string PostalCode { get; }


    private Address(string street, string city, string state, string country, string postalCode)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
    }

    public static Address Create(string street, string city, string state, string country, string postalCode)
    {
        Validate(street, city, state, country, postalCode);

        return new(street.Trim(), city.Trim(), state.Trim(), country.Trim(), postalCode.Trim());
    }

    private static void Validate(string street, string city, string state, string country, string postalCode)
    {
        if (street.IsNullOrEmpty() || city.IsNullOrEmpty() ||
            state.IsNullOrEmpty() || country.IsNullOrEmpty() || postalCode.IsNullOrEmpty())
            throw new InvalidAddressException("Street, City, State, Country and Postal code can not be null or empty in address!");
    }

    public static implicit operator string(Address address)=> address.ToString();

    public override string ToString()
    {
        if (this.Equals(Unknown))
            return string.Empty;

        return $"{Street}, {City}, {State}, {Country}, {PostalCode}";
    }
}