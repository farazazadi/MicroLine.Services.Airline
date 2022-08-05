namespace MicroLine.Services.Airline.Domain.Common.Exceptions;

public class InvalidMoneyOperationException : DomainException
{
    public override string Code => nameof(InvalidMoneyOperationException);

    public InvalidMoneyOperationException(string message) : base(message)
    {
    }
}