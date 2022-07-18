using MicroLine.Services.Airline.Domain.Airport.Exceptions;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.Extensions;

namespace MicroLine.Services.Airline.Domain.Airport;

public class AirportLocation : ValueObject
{
    public Continent Continent { get; }
    public string Country { get; }
    public string Region { get; }
    public string City { get; }
    public decimal Latitude { get; }
    public decimal Longitude { get; }

    private AirportLocation(Continent continent, string country, string region, string city,
                            decimal latitude, decimal longitude)
    {
        Continent = continent;
        Country = country;
        Region = region;
        City = city;
        Latitude = latitude;
        Longitude = longitude;
    }



    public static AirportLocation Create(Continent continent, string country, string region, string city,
                                            decimal latitude, decimal longitude)
    {
        Validate(country, region, city, latitude, longitude);

        return new (continent, country.Trim(), region.Trim(), city.Trim(),
                                latitude, longitude);
    }

    private static void Validate(string country, string region, string city, decimal latitude, decimal longitude)
    {
        if (country.IsNullOrEmpty() || region.IsNullOrEmpty() || city.IsNullOrEmpty())
            throw new InvalidAirportLocationException("Country, Region and City can not be null or empty in AirportLocation!");

        if(!IsValidLatitude(latitude))
            throw new InvalidAirportLocationException($"Latitude ({latitude}) is not valid in AirportLocation!");


        if (!IsValidLongitudee(longitude))
            throw new InvalidAirportLocationException($"Longitude ({longitude}) is not valid in AirportLocation!");
    }

    private static bool IsValidLatitude(decimal latitude) => Math.Truncate(latitude) is >= (-90) and <= 90;
    private static bool IsValidLongitudee(decimal longitude) => Math.Truncate(longitude) is >= (-180) and <= 180;


    public static implicit operator string(AirportLocation address) => address.ToString();

    public override string ToString() => $"{Country}, {Region}, {City}, [{Latitude}, {Longitude}]";
}
