using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.Flights;

namespace MicroLine.Services.Airline.Domain.FlightPricingPolicies;
public class WeekDayFlightPricingPolicy : IFlightPricingPolicy
{
    public byte Priority => 0;

    private readonly Dictionary<DayOfWeek, decimal> _weekDaysRatioDictionary = new()
    {
        {DayOfWeek.Monday, 1.04m},
        {DayOfWeek.Tuesday, 0.91m},
        {DayOfWeek.Wednesday, 0.93m},
        {DayOfWeek.Thursday, 1.06m},
        {DayOfWeek.Friday, 1.07m},
        {DayOfWeek.Saturday, 1.03m},
        {DayOfWeek.Sunday, 1.20m}
    };

    private WeekDayFlightPricingPolicy()
    {
    }

    public static WeekDayFlightPricingPolicy Create ()=> new WeekDayFlightPricingPolicy();

    public FlightPrice Calculate(Flight flight)
    {
        var weekDay = flight.ScheduledUtcDateTimeOfDeparture.DayOfWeek;
        var prices = flight.Prices;
        var currency = flight.Prices.BusinessClass.Currency;

        var baseEconomyClassAmount = prices.EconomyClass.Amount;
        var baseBusinessClassAmount = prices.BusinessClass.Amount;
        var baseFirstClassAmount = prices.FirstClass.Amount;
        
        var economyClassAmount = baseEconomyClassAmount * _weekDaysRatioDictionary[weekDay];
        var businessClassAmount = baseBusinessClassAmount * _weekDaysRatioDictionary[weekDay];
        var firstClassAmount = baseFirstClassAmount * _weekDaysRatioDictionary[weekDay];

        return FlightPrice.Create(
            Money.Of(economyClassAmount, currency),
            Money.Of(businessClassAmount, currency),
            Money.Of(firstClassAmount, currency)
            );
    }
}
