namespace MicroLine.Services.Airline.Domain.Common;

public abstract class AuditableEntity : Entity
{
    public string CreatedBy { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public string LastModifiedBy { get; private set; }

    public DateTime? LastModifiedAtUtc { get; private set; }


    public void SetModificationDetails(string userId, DateTime utcDateTimeOfModification)
    {
        LastModifiedBy = userId;
        LastModifiedAtUtc = utcDateTimeOfModification;
    }

    public void SetCreationDetails(string userId, DateTime utcDateTimeOfCreation)
    {
        CreatedBy = userId;
        CreatedAtUtc = utcDateTimeOfCreation;
    }
}