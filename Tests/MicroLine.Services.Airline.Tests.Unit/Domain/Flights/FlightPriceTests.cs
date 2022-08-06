using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.Flights;
using MicroLine.Services.Airline.Domain.Flights.Exceptions;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Flights;
public class FlightPriceTests
{
    [Fact]
    public void FlightPrice_ShouldThrowInvalidFlightPriceException_WhenItCreatesFromMoneysWithDifferentCurrencyTypes()
    {
        // Given
        var economyClassPrice = Money.Of(100.50m, Money.CurrencyType.Euro);
        var businessClassPrice = Money.Of(200, Money.CurrencyType.UnitedStatesDollar);
        var firstClassPrice = Money.Of(500, Money.CurrencyType.Euro);

        // When
        var func = () =>  FlightPrice.Create(economyClassPrice, businessClassPrice, firstClassPrice);


        // Then
        func.Should().ThrowExactly<InvalidFlightPriceException>()
            .And.Code.Should().Be(nameof(InvalidFlightPriceException));
    }
}
