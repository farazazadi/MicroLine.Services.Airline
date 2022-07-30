using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.Airports.Exceptions;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Airports;

public class IataCodeTests
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
    public void IataCode_ShouldThrowInvalidIataCodeException_WhenItCreatesFromNullOrEmptyString(string iataCode)
    {
        // Given
        // When
        var func = () => IataCode.Create(iataCode);

        // Then
        func.Should().ThrowExactly<InvalidIataCodeException>()
            .And.Code.Should().Be(nameof(InvalidIataCodeException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void IataCode_ShouldThrowInvalidIataCodeException_WhenItConvertsFromNullOrEmptyString(string iataCode)
    {
        // Given
        // When
        var createByImplicitConversationFromString = () => { IataCode code = iataCode; };

        // Then
        createByImplicitConversationFromString.Should().ThrowExactly<InvalidIataCodeException>()
            .And.Code.Should().Be(nameof(InvalidIataCodeException));
    }



    public static TheoryData<string> IataCodeStringsWithNonEnglishCharacters = new()
    {
        "YJف",
        "YJ ",
        "YY1",
        "YYφ",
    };

    [Theory, MemberData(nameof(IataCodeStringsWithNonEnglishCharacters))]
    public void IataCode_ShouldThrowInvalidIataCodeException_WhenItCreatesFromStringWithNoneEnglishCharacters(string iataCode)
    {
        // Given
        // When
        var func = () => IataCode.Create(iataCode);

        // Then
        func.Should().ThrowExactly<InvalidIataCodeException>()
            .And.Code.Should().Be(nameof(InvalidIataCodeException));
    }



    public static TheoryData<string> IataCodeStringsWithInvalidLength = new()
    {
        "JJ",
        "YYYJ             ",
        "YYJJ",
        "           YY",
    };

    [Theory, MemberData(nameof(IataCodeStringsWithInvalidLength))]
    public void IataCode_ShouldThrowInvalidIataCodeException_WhenItCreatesFromStringWithLengthLessThanOrGreaterThan3(string iataCode)
    {
        // Given
        // When
        var func = () => IataCode.Create(iataCode);

        // Then
        func.Should().ThrowExactly<InvalidIataCodeException>()
            .And.Code.Should().Be(nameof(InvalidIataCodeException));
    }


    public static TheoryData<string> ValidIataCodeStrings = new()
    {
        "YYJ",
        "YYT",
        "yul",
        "Yvr",
    };
    [Theory, MemberData(nameof(ValidIataCodeStrings))]
    public void IataCode_ShouldHaveUpperCaseToString_WhenItCreatesFromValidString(string iataCode)
    {
        // Given
        // When
        var code = IataCode.Create(iataCode);

        // Then
        code.ToString().Should().Be(iataCode.ToUpperInvariant());
    }

}