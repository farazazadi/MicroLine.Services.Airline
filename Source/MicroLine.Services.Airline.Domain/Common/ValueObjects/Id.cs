using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Common.ValueObjects;

public sealed class Id : ValueObject
{
    public bool IsTransient => Value == Guid.Empty;
    public Guid Value { get; }

    public static Id Transient => new(Guid.Empty);

    private Id(Guid guid) => Value = guid;

    public static Id Create() => Create(Guid.NewGuid());

    public static Id Create(Guid guid)
    {
        Validate(guid);

        return new(guid);
    }

    private static void Validate(Guid guid)
    {
        if (guid == Guid.Empty)
            throw new InvalidIdException(guid);
    }


    public static implicit operator Guid(Id id) => id.Value;

    public static implicit operator Id(Guid id) => new(id);

    public override string ToString() => Value.ToString();
}