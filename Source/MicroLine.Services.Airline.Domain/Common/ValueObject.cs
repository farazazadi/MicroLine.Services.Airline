using System.Reflection;
using MicroLine.Services.Airline.Domain.Common.Attributes;
using MicroLine.Services.Airline.Domain.Common.Extensions;

namespace MicroLine.Services.Airline.Domain.Common;

[Serializable]
public abstract class ValueObject : IComparable, IComparable<ValueObject>
{
    private int? _cachedHashCode;

    private List<object> _properties;

    private List<object> _fields;

    private List<object> _equalityComponents;

    private List<object> Properties
    {
        get
        {
            _properties ??= GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(propertyInfo => !Attribute.IsDefined(propertyInfo, typeof(IgnoreMemberAttribute)))
                .Select(propertyInfo => propertyInfo.GetValue(this, null))
                .ToList();

            return _properties;
        }
    }

    private List<object> Fields
    {
        get
        {
            _fields ??= GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(fieldInfo => !Attribute.IsDefined(fieldInfo, typeof(IgnoreMemberAttribute)))
                .Select(fieldInfo => fieldInfo.GetValue(this))
                .ToList();

            return _fields;
        }
    }

    private List<object> EqualityComponents
    {
        get
        {
            _equalityComponents ??= Properties.Concat(Fields).ToList();

            return _equalityComponents;
        }
    }


    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (this.GetRealType() != obj.GetRealType())
            return false;

        var valueObject = (ValueObject)obj;

        var areEqual = EqualityComponents
            .SequenceEqual(valueObject.EqualityComponents);

        return areEqual;

    }


    public int CompareTo(object obj)
    {
        var thisType = this.GetRealType();
        var otherType = obj.GetRealType();

        if (thisType != otherType)
            return string.Compare(thisType.ToString(), otherType.ToString(), StringComparison.Ordinal);

        var other = (ValueObject)obj;

        var components = EqualityComponents.ToArray();
        var otherComponents = other.EqualityComponents.ToArray();

        for (var i = 0; i < components.Length; i++)
        {
            var comparison = CompareComponents(components[i], otherComponents[i]);
                
            if (comparison != 0)
                return comparison;
        }

        return 0;
    }

    private int CompareComponents(object object1, object object2)
    {
        if (object1 is null && object2 is null)
            return 0;

        if (object1 is null)
            return -1;

        if (object2 is null)
            return 1;

        if (object1 is IComparable comparable1 && object2 is IComparable comparable2)
            return comparable1.CompareTo(comparable2);

        return object1.Equals(object2) ? 0 : -1;
    }


    public int CompareTo(ValueObject other) => CompareTo(other as object);


    public override int GetHashCode()
    {
        _cachedHashCode ??= EqualityComponents
            .Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return current * 23 + (obj?.GetHashCode() ?? 0);
                }
            });

        return _cachedHashCode.Value;
    }

    public static bool operator ==(ValueObject a, ValueObject b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject a, ValueObject b) => !(a == b);
}