using FluentAssertions;
using MicroLine.Services.Airline.Domain.Airport;
using MicroLine.Services.Airline.Domain.Airport.Exceptions;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Airport;

public class IcaoCodeTests
{
    public static TheoryData<string> NullOrEmptyStrings = new()
    {
        "",
        " ",
        "        ",
        string.Empty,
        null
    };

    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void IcaoCode_ShouldThrowInvalidIcaoCodeException_WhenItCreatesFromNullOrEmptyString(string icaoCode)
    {
        // Given
        // When
        var func = () => IcaoCode.Create(icaoCode);

        // Then
        func.Should().ThrowExactly<InvalidIcaoCodeException>()
            .And.Code.Should().Be(nameof(InvalidIcaoCodeException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void IcaoCode_ShouldThrowInvalidIcaoCodeException_WhenItConvertsFromNullOrEmptyString(string icaoCode)
    {
        // Given
        // When
        var createByImplicitConversationFromString = () => { IcaoCode code = icaoCode; };

        // Then
        createByImplicitConversationFromString.Should().ThrowExactly<InvalidIcaoCodeException>()
            .And.Code.Should().Be(nameof(InvalidIcaoCodeException));
    }



    public static TheoryData<string> IcaoCodeStringsWithNonEnglishCharacters = new()
    {
        "CYYJف",
        "CYYJ ",
        "CYY1",
        "CYYφ",
    };

    [Theory, MemberData(nameof(IcaoCodeStringsWithNonEnglishCharacters))]
    public void IcaoCode_ShouldThrowInvalidIcaoCodeException_WhenItCreatesFromStringWithNoneEnglishCharacters(string icaoCode)
    {
        // Given
        // When
        var func = () => IcaoCode.Create(icaoCode);

        // Then
        func.Should().ThrowExactly<InvalidIcaoCodeException>()
            .And.Code.Should().Be(nameof(InvalidIcaoCodeException));
    }



    public static TheoryData<string> IcaoCodeStringsWithInvalidLength = new()
    {
        "CYY",
        "CYYJJ",
        "CYY             ",
        "           CYY",
    };

    [Theory, MemberData(nameof(IcaoCodeStringsWithInvalidLength))]
    public void IcaoCode_ShouldThrowInvalidIcaoCodeException_WhenItCreatesFromStringWithLengthLessThanOrGreaterThan4(string icaoCode)
    {
        // Given
        // When
        var func = () => IcaoCode.Create(icaoCode);

        // Then
        func.Should().ThrowExactly<InvalidIcaoCodeException>()
            .And.Code.Should().Be(nameof(InvalidIcaoCodeException));
    }


    public static TheoryData<string> ValidIcaoCodeStrings = new()
    {
        "CYYJ",
        "CYUL",
        "CyHz",
        "cyeg",
    };
    [Theory, MemberData(nameof(ValidIcaoCodeStrings))]
    public void IcaoCode_ShouldHaveUpperCaseToString_WhenItCreatesFromValidString(string icaoCode)
    {
        // Given
        // When
        var code = IcaoCode.Create(icaoCode);

        // Then
        code.ToString().Should().Be(icaoCode.ToUpperInvariant());
    }

}