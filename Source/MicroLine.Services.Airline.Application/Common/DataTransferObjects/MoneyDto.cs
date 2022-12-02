using static MicroLine.Services.Airline.Domain.Common.ValueObjects.Money;

namespace MicroLine.Services.Airline.Application.Common.DataTransferObjects;

public record MoneyDto(
    decimal Amount,
    CurrencyType Currency
);