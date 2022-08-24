using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Configurations.ValueConverters;

internal class IdConvertor : ValueConverter<Id, Guid>
{
    public IdConvertor() : base(
        id => id.IsTransient ? Id.Create().Value : id.Value,
        guid => guid)
    {
    }
}