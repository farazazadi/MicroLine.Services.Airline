using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.Extensions;

namespace MicroLine.Services.Airline.Domain.Common.ValueObjects;

public sealed class Speed : ValueObject
{
    public enum UnitOfSpeed
    {
        KilometresPerHour = 0, // kmh km/h
        MilesPerHour = 1, // mph
        Knot = 2, //kn
    }


    private static readonly Dictionary<UnitOfSpeed , string> UnitOfSpeedSymbols = new()
    {
        {UnitOfSpeed.KilometresPerHour, "Km/h"},
        {UnitOfSpeed.MilesPerHour, "mph"},
        {UnitOfSpeed.Knot, "Kn"}
    };

    public UnitOfSpeed Unit { get; }
    public int Value { get; }

    private Speed(int value, UnitOfSpeed unitOfSpeed)
    {
        Unit = unitOfSpeed;
        Value = value;
    }

    public static Speed Create(int speed, UnitOfSpeed unitOfSpeed)
    {
        Validate(speed);

        return new Speed(speed, unitOfSpeed);
    }


    private static Speed Create(string speed)
    {
        Validate(speed);

        var speedParts = GetSpeedStringParts(speed);

        var value = int.Parse(speedParts[0]);
        var unitOfSpeed = UnitOfSpeedSymbols.First(p=> p.Value == speedParts[1]).Key;

        return new Speed(value, unitOfSpeed);
    }

    private static void Validate(int speed)
    {
        if (speed < 0)
            throw new InvalidSpeedException(speed);
    }

    private static void Validate(string speed)
    {
        if(speed.IsNullOrEmpty())
            throw new InvalidSpeedException(speed);

        var speedParts = GetSpeedStringParts(speed);

        if(speedParts.Length != 2)
            throw new InvalidSpeedException(speed);

        if(!int.TryParse(speedParts[0], out var value))
            throw new InvalidSpeedException(speed);

        Validate(value);

        var unitExist = UnitOfSpeedSymbols.Any(p=> p.Value == speedParts[1]);

        if(!unitExist)
            throw new InvalidSpeedException(speed);
    }

    private static string[] GetSpeedStringParts (string speed) => speed.Split(" ", StringSplitOptions.RemoveEmptyEntries);


    public static implicit operator Speed(string speed) => Create(speed);
    public static implicit operator string(Speed speed) => speed.ToString();

    public override string ToString() => $"{Value} {UnitOfSpeedSymbols[Unit]}";
}