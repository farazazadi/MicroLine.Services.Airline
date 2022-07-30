using MicroLine.Services.Airline.Domain.Aircraft.Exceptions;
using MicroLine.Services.Airline.Domain.Common;

namespace MicroLine.Services.Airline.Domain.Aircraft;

public sealed class PassengerSeatingCapacity : ValueObject
{
    public int EconomyClassCapacity { get; }
    public int BusinessClassCapacity { get; }
    public int FirstClassCapacity { get; }

    public int TotalCapacity => EconomyClassCapacity + BusinessClassCapacity + FirstClassCapacity;

    private PassengerSeatingCapacity(int economyClassCapacity, int businessClassCapacity, int firstClassCapacity)
    {
        EconomyClassCapacity = economyClassCapacity;
        BusinessClassCapacity = businessClassCapacity;
        FirstClassCapacity = firstClassCapacity;
    }

    public static PassengerSeatingCapacity Create(int economyClassCapacity, int businessClassCapacity, int firstClassCapacity)
    {
        Validate(economyClassCapacity, businessClassCapacity, firstClassCapacity);

        return new PassengerSeatingCapacity(economyClassCapacity, businessClassCapacity, firstClassCapacity);
    }

    private static void Validate(int economyClassCapacity, int businessClassCapacity, int firstClassCapacity)
    {
        if(economyClassCapacity < 0)
            throw new InvalidPassengerSeatingCapacityException("Count of economy class seats can not be negative!");
        if(businessClassCapacity < 0)
            throw new InvalidPassengerSeatingCapacityException("Count of business class seats can not be negative!");
        if (firstClassCapacity < 0)
            throw new InvalidPassengerSeatingCapacityException("Count of business class seats can not be negative!");

        if (economyClassCapacity + businessClassCapacity + firstClassCapacity < 1)
            throw new InvalidPassengerSeatingCapacityException("passenger seating capacity should be greater than 0!");
    }

    public override string ToString() =>
        $"{TotalCapacity} (Economy class: {EconomyClassCapacity}, Business Class: {BusinessClassCapacity}, First Class: {FirstClassCapacity})";
}