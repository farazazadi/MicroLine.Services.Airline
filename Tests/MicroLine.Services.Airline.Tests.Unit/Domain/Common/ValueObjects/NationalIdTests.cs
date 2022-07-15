using FluentAssertions;
using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Common.ValueObjects;
public class NationalIdTests
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
    public void NationalId_ShouldThrowInvalidNationalIdException_WhenItCreatesFromNullOrEmptyInput(string nationalId)
    {
        // Given
        // When
        var action = () => NationalId.Create(nationalId);

        // Then
        action.Should().ThrowExactly<InvalidNationalIdException>()
            .And.Code.Should().Be(nameof(InvalidNationalIdException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void NationalId_ShouldThrowInvalidNationalIdException_WhenItConvertsFromNullOrEmptyInput(string nationalId)
    {
        // Given
        // When
        var action = () => { var id = (NationalId)nationalId; };

        // Then
        action.Should().ThrowExactly<InvalidNationalIdException>()
            .And.Code.Should().Be(nameof(InvalidNationalIdException));
    }



    public static TheoryData<string> NationalIdsWithInvalidLength = new()
    {
        "1",
        "123",
        "AF12345",
        "BX-123456789102345678920",
        "1234567891023456789206666666666666666"
    };

    [Theory, MemberData(nameof(NationalIdsWithInvalidLength))]
    public void NationalId_ShouldThrowInvalidNationalIdException_WhenItCreatesFromStringWithLengthLessThan8OrGreaterThan20(string nationalId)
    {
        // Given
        // When
        var action = () => NationalId.Create(nationalId);

        // Then
        action.Should().ThrowExactly<InvalidNationalIdException>()
            .And.Code.Should().Be(nameof(InvalidNationalIdException));
    }


    [Theory, MemberData(nameof(NationalIdsWithInvalidLength))]
    public void NationalId_ShouldThrowInvalidNationalIdException_WhenItConvertsFromStringWithLengthLessThan8OrGreaterThan20(string nationalId)
    {
        // Given
        // When
        var action = () => { var id = (NationalId)nationalId; };

        // Then
        action.Should().ThrowExactly<InvalidNationalIdException>()
            .And.Code.Should().Be(nameof(InvalidNationalIdException));
    }



    [Fact]
    public void NationalId_ShouldHaveValidToStringOutput_WhenItCreatedFromValidInput()
    {
        // Given
        var id = "AX1234567890";

        // When
        var nationalId = NationalId.Create(id);

        // Then
        nationalId.ToString().Should().Be(id);
    }


    [Fact]
    public void NationalId_ShouldHaveEmptyToStringOutput_WhenItCreatesAsUnknown()
    {
        // Given
        // When
        var nationalId = NationalId.Unknown;

        // Then
        nationalId.ToString().Should().Be(string.Empty);

    }

}