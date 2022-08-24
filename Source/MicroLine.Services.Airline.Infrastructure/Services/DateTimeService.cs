using MicroLine.Services.Airline.Application.Common.Contracts;

namespace MicroLine.Services.Airline.Infrastructure.Services;
internal class DateTimeService : IDateTime
{
    public DateTimeOffset Now => DateTimeOffset.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
