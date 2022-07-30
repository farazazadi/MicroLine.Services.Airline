using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Common.ValueObjects;

public class PassportNumberTests
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
    public void PassportNumber_ShouldThrowInvalidPassportNumberException_WhenItCreatesFromNullOrEmptyInput(string passportNumber)
    {
        // Given
        // When
        var action = () => PassportNumber.Create(passportNumber);

        // Then
        action.Should().ThrowExactly<InvalidPassportNumberException>()
            .And.Code.Should().Be(nameof(InvalidPassportNumberException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void PassportNumber_ShouldThrowInvalidPassportNumberException_WhenItConvertsFromNullOrEmptyInput(string passportNumber)
    {
        // Given
        // When
        var action = () => { var id = (PassportNumber)passportNumber; };

        // Then
        action.Should().ThrowExactly<InvalidPassportNumberException>()
            .And.Code.Should().Be(nameof(InvalidPassportNumberException));
    }



    public static TheoryData<string> PassportNumbersWithInvalidLength = new()
    {
        "1",
        "123",
        "A2358",
        "12345",
        "A123456789",
    };

    [Theory, MemberData(nameof(PassportNumbersWithInvalidLength))]
    public void PassportNumber_ShouldThrowInvalidPassportNumberException_WhenItCreatesFromStringWithLengthLessThan6OrGreaterThan9(string passportNumber)
    {
        // Given
        // When
        var action = () => PassportNumber.Create(passportNumber);

        // Then
        action.Should().ThrowExactly<InvalidPassportNumberException>()
            .And.Code.Should().Be(nameof(InvalidPassportNumberException));
    }


    [Theory, MemberData(nameof(PassportNumbersWithInvalidLength))]
    public void PassportNumber_ShouldThrowInvalidPassportNumberException_WhenItConvertsFromStringWitthLengthLessThan6OrGreaterThan9(string passportNumber)
    {
        // Given
        // When
        var action = () => { var id = (PassportNumber)passportNumber; };

        // Then
        action.Should().ThrowExactly<InvalidPassportNumberException>()
            .And.Code.Should().Be(nameof(InvalidPassportNumberException));
    }



    public static TheoryData<string> InvalidPassportNumbersContainingNonLetterOrDigitCharacters = new()
    {
        "A1234567*",
        "-A1234567",
        "$A1234567",
        "B1234.567"
    };

    [Theory, MemberData(nameof(InvalidPassportNumbersContainingNonLetterOrDigitCharacters))]
    public void PassportNumber_ShouldThrowInvalidPassportNumberException_WhenItCreatesFromInvalidStringThatContainsNonLetterOrDigitCharacters(string passportNumber)
    {
        // Given
        // When
        var action = () => PassportNumber.Create(passportNumber);

        // Then
        action.Should().ThrowExactly<InvalidPassportNumberException>()
            .And.Code.Should().Be(nameof(InvalidPassportNumberException));
    }


    [Theory, MemberData(nameof(InvalidPassportNumbersContainingNonLetterOrDigitCharacters))]
    public void PassportNumber_ShouldThrowInvalidPassportNumberException_WhenItConvertsFromInvalidStringThatContainsNonLetterOrDigitCharacters(string passportNumber)
    {
        // Given
        // When
        var action = () => { var id = (PassportNumber)passportNumber; };

        // Then
        action.Should().ThrowExactly<InvalidPassportNumberException>()
            .And.Code.Should().Be(nameof(InvalidPassportNumberException));
    }



    [Fact]
    public void PassportNumber_ShouldHaveValidToStringOutput_WhenItCreatedFromValidInput()
    {
        // Given
        var passNumber = "A12345678";

        // When
        var passportNumber = PassportNumber.Create(passNumber);

        // Then
        passportNumber.ToString().Should().Be(passNumber);
    }

}