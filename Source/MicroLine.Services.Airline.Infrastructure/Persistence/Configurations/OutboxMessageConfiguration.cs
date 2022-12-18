using MicroLine.Services.Airline.Infrastructure.Persistence.Configurations.Extensions;
using MicroLine.Services.Airline.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Configurations;

internal class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.Property(message => message.Subject)
            .IsRequired();

        builder.Property(message => message.Content)
            .IsRequired();

        builder.AddRowVersion();
    }
}
