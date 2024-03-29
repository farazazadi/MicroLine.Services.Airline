﻿using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Aircrafts.Exceptions;

public class InvalidPassengerSeatingCapacityException : DomainException
{
    public override string Code => nameof(InvalidPassengerSeatingCapacityException);

    public InvalidPassengerSeatingCapacityException(string message) : base(message)
    {
    }
}
