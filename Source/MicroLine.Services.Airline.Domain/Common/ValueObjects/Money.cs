
using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Common.ValueObjects;
public class Money : ValueObject
{
    public decimal Amount { get; }
    public CurrencyType Currency { get; }

    public enum CurrencyType
    {
        UnitedStatesDollar = 0,
        AustralianDollar = 1,
        CanadianDollar = 2,
        PoundSterling = 3,
        Euro = 4
    }

    private static readonly Dictionary<CurrencyType, string> CurrencySymbolsDictionary = new()
    {
        {CurrencyType.UnitedStatesDollar, "$"},
        {CurrencyType.AustralianDollar, "AUD"},
        {CurrencyType.CanadianDollar, "CAD"},
        {CurrencyType.PoundSterling, "£"},
        {CurrencyType.Euro, "€"},
    };

    private Money(decimal amount, CurrencyType currency)
    {
        Currency = currency;
        Amount = amount;
    }

    public static Money Of(decimal amount, CurrencyType currency)
    {
        Validate(amount, currency);

        return new Money(amount, currency);
    }

    private static void Validate(decimal amount, CurrencyType currency)
    {
        if (amount < 0)
            throw new InvalidMoneyException("Amount of money can not be negative!");
    }

    private static void Validate(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidMoneyOperationException("Both money should have same CurrencyType!");
    }


    public static Money operator *(Money left, Money right)
    {
        Validate(left, right);

        var amount = left.Amount * right.Amount;
        var currency = left.Currency;

        return Of(amount, currency);
    }


    public static Money operator /(Money left, Money right)
    {
        Validate(left, right);

        var amount = left.Amount / right.Amount;
        var currency = left.Currency;

        return Of(amount, currency);
    }


    public static Money operator +(Money left, Money right)
    {
        Validate(left, right);

        var amount = left.Amount + right.Amount;
        var currency = left.Currency;

        return Of(amount, currency);
    }


    public static Money operator -(Money left, Money right)
    {
        Validate(left, right);

        var amount = left.Amount - right.Amount;
        var currency = left.Currency;

        return Of(amount, currency);
    }


    public static bool operator >(Money left, Money right)
    {
        Validate(left, right);
        return left.Amount > right.Amount;
    }

    public static bool operator >=(Money left, Money right)
    {
        Validate(left, right);
        return left.Amount >= right.Amount;
    }

    public static bool operator <(Money left, Money right)
    {
        Validate(left, right);
        return left.Amount < right.Amount;
    }

    public static bool operator <=(Money left, Money right)
    {
        Validate(left, right);
        return left.Amount < right.Amount;
    }


    public override string ToString() => $"{CurrencySymbolsDictionary[Currency]}{Amount}";
}
