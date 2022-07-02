using MicroLine.Services.Airline.Domain.Common.Extensions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Domain.Common;

[Serializable]
public abstract class Entity
{
    public Id Id { get; protected set; } = Id.Transient;

    public override bool Equals(object obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (this.GetRealType() != other.GetRealType())
            return false;

        if (IsTransient() || other.IsTransient())
            return false;

        return Id.Equals(other.Id);
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    private bool IsTransient()
    {
        if (Id is null)
            return true;
        
        return Id.IsTransient;
    }

    public static bool operator !=(Entity a, Entity b) => !(a == b);

    public override int GetHashCode() => (this.GetRealType().ToString() + Id).GetHashCode();
}
