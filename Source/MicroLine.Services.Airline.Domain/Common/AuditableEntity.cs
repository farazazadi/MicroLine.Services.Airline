namespace MicroLine.Services.Airline.Domain.Common;

public abstract class AuditableEntity : Entity
{
    public string CreatedBy { get; private set; }

    public DateTimeOffset CreatedUtc { get; private set; }

    public string LastModifiedBy { get; private set; }

    public DateTimeOffset? LastModifiedUtc { get; private set; }

}