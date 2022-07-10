using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Common.ValueObjects;

public sealed class Date : ValueObject
{
    public int Year { get; }
    public int Month { get; }
    public int Day { get; }

    private readonly DateOnly _dateOnly;

    private Date(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;

        _dateOnly = new DateOnly(Year, Month, Day);
    }


    public static Date Create(string dateTimeString)
    {
        if (!DateTime.TryParse(dateTimeString?.Trim(), out var dateTime))
            throw new InvalidDateException(dateTimeString);

        return Create(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    public static Date Create(int year, int month, int day)
    {
        Validate(year, month, day);

        return new Date(year, month, day);
    }

    private static void Validate(int year, int month, int day)
    {
        try
        {
            var date = new DateOnly(year, month, day);
        }
        catch
        {
            throw new InvalidDateException(year, month, day);
        }

        if (year is < 1900 or > 2100 || month is < 1 or > 12 || day is < 1 or > 31)
            throw new InvalidDateException(year, month, day);
    }


    public static implicit operator Date(DateOnly dateOnly) => Create(dateOnly.Year, dateOnly.Month, dateOnly.Day);
    public static implicit operator Date(DateTime dateTime) => Create(dateTime.Year, dateTime.Month, dateTime.Day);
    public static implicit operator Date(string dateTime) => Create(dateTime);

    public static implicit operator DateOnly(Date date) => new(date.Year, date.Month, date.Day);
    public static implicit operator DateTime(Date date) => new(date.Year, date.Month, date.Day);
    public static implicit operator string(Date date) => date.ToString();

    public override string ToString() => _dateOnly.ToString("yyyy/MM/dd");

}