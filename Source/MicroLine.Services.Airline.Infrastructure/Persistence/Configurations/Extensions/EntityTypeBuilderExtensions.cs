using MicroLine.Services.Airline.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Configurations.Extensions;

internal static class EntityTypeBuilderExtensions
{
    internal static PropertyBuilder<byte[]> AddRowVersion<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Entity
    {
        return builder.Property<byte[]>("RowVersion")
            .IsRowVersion();
    }
}