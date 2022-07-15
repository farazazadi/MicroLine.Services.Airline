using MicroLine.Services.Airline.Domain.Common.Exceptions;
using System.Text.RegularExpressions;

namespace MicroLine.Services.Airline.Domain.Common.ValueObjects;

public sealed class Email : ValueObject
{
    public static Email Unknown => new(string.Empty);

    private readonly string _emailAddress;

    private Email(string emailAddress)
    {
        _emailAddress = emailAddress;
    }

    public static Email Create(string emailAddress)
    {
        Validate(emailAddress);

        return new Email(emailAddress.ToLowerInvariant());
    }


    private static void Validate(string emailAddress)
    {
        if (string.IsNullOrEmpty(emailAddress))
            throw new InvalidEmailException("Email address can not be null or empty!");

        var validEmailPattern = @"^\s*[\w\-\+_']+(\.[\w\-\+_']+)*\@[A-Za-z0-9]([\w\.-]*[A-Za-z0-9])?\.[A-Za-z][A-Za-z\.]*[A-Za-z]$";

        var isMatch = Regex.IsMatch(emailAddress, validEmailPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        if(isMatch is false)
            throw new InvalidEmailException($"Email address is not valid!");

    }

    public static implicit operator Email(string email) => Create(email);
    public static implicit operator string(Email email) => email.ToString();
    public override string ToString() => _emailAddress;
}