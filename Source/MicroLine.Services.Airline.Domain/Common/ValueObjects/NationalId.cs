using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.Extensions;

namespace MicroLine.Services.Airline.Domain.Common.ValueObjects;
public sealed class NationalId : ValueObject
{
    public static NationalId Unknown => new (string.Empty);

    private readonly string _nationalId;

    private NationalId(string nationalId)
    {
        _nationalId = nationalId;
    }

    public static NationalId Create(string id)
    {
        Validate(id);

        return new NationalId(id.Trim());
    }

    private static void Validate(string id)
    {
        if (id.IsNullOrEmpty())
            throw new InvalidNationalIdException("National Id can not be null or empty!");

        if (!id.HasValidLength(8, 20))
            throw new InvalidNationalIdException("Length of National Id should be greater than 8 and less than or equal to 20 characters!");
    }


    public static implicit operator NationalId(string nationalId) => Create(nationalId);
    public static implicit operator string(NationalId nationalId) => nationalId.ToString();
    public override string ToString() => _nationalId;
}
