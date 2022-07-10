namespace MicroLine.Services.Airline.Domain.Common.Exceptions;

public class InvalidDateException : DomainException
{
    public override string Code => nameof(InvalidDateException);

    public InvalidDateException(int year, int month, int day) : base($"Invalid date (Year/Month/Day): {year}/{month}/{day} !")
    {
    }

    public InvalidDateException(string dateTime) : base($"Date can not be created with invalid ({dateTime}) DateTime string!")
    {
    }
}