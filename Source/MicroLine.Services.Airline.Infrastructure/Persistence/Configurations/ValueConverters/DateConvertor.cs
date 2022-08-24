using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Configurations.ValueConverters;

internal class DateConvertor : ValueConverter<Date, DateTime>
{
    public DateConvertor() : base(
        date => date,
        dateTime => dateTime)
    {
    }
}