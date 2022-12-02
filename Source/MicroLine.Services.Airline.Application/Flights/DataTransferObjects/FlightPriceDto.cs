using MicroLine.Services.Airline.Application.Common.DataTransferObjects;

namespace MicroLine.Services.Airline.Application.Flights.DataTransferObjects;

public record FlightPriceDto(
    MoneyDto EconomyClass,
    MoneyDto BusinessClass,
    MoneyDto FirstClass
);