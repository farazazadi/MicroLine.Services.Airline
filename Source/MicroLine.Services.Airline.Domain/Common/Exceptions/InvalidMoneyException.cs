namespace MicroLine.Services.Airline.Domain.Common.Exceptions;
public class InvalidMoneyException : DomainException
{
    public override string Code => nameof(InvalidMoneyException);

    public InvalidMoneyException(string message) : base(message)
    {
    }
}