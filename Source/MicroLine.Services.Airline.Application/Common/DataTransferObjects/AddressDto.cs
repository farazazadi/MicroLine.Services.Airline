namespace MicroLine.Services.Airline.Application.Common.DataTransferObjects;
public record AddressDto(
    string Street,
    string City,
    string State,
    string Country,
    string PostalCode
    );