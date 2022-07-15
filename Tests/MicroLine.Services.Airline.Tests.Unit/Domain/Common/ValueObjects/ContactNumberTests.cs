using FluentAssertions;
using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Common.ValueObjects;

public class ContactNumberTests
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
    public void ContactNumber_ShouldThrowInvalidContactNumberException_WhenItCreatesFromNullOrEmptyInput(string contactNumber)
    {
        // Given
        // When
        var action = () => ContactNumber.Create(contactNumber);

        // Then
        action.Should().ThrowExactly<InvalidContactNumberException>()
            .And.Code.Should().Be(nameof(InvalidContactNumberException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void ContactNumber_ShouldThrowInvalidContactNumberException_WhenItConvertsFromNullOrEmptyInput(string contactNumber)
    {
        // Given
        // When
        var action = () => { var id = (ContactNumber)contactNumber; };

        // Then
        action.Should().ThrowExactly<InvalidContactNumberException>()
            .And.Code.Should().Be(nameof(InvalidContactNumberException));
    }


    // Valid Contact number starts with + or 00 and continues with minimum 10 and maximum 15 digits without any extra characters
    public static TheoryData<string> InvalidContactNumbers = new()
    {
        "+530123456",
        "00530123456",
        "123",
        "00123",
        "+123",
        "005301234566666666",
        "+5301234566666666",
        "++5301234566",
        "+005301234566",
        "+530-1234566",
        "+530D1234566"
    };

    [Theory, MemberData(nameof(InvalidContactNumbers))]
    public void ContactNumber_ShouldThrowInvalidContactNumberException_WhenItCreatesFromInputWithInvalidFormat(string contactNumber)
    {
        // Given
        // When
        var action = () => ContactNumber.Create(contactNumber);

        // Then
        action.Should().ThrowExactly<InvalidContactNumberException>()
            .And.Code.Should().Be(nameof(InvalidContactNumberException));
    }


    [Theory, MemberData(nameof(InvalidContactNumbers))]
    public void ContactNumber_ShouldThrowInvalidContactNumberException_WhenItConvertsFromInputWithInvalidFormat(string contactNumber)
    {
        // Given
        // When
        var action = () => { var id = (ContactNumber)contactNumber; };

        // Then
        action.Should().ThrowExactly<InvalidContactNumberException>()
            .And.Code.Should().Be(nameof(InvalidContactNumberException));
    }





    [Fact]
    public void ContactNumber_ShouldHaveValidToStringOutput_WhenItCreatedFromValidInput()
    {
        // Given
        var number = "+12233445568";

        // When
        var contactNumber = ContactNumber.Create(number);

        // Then
        contactNumber.ToString().Should().Be(number);
    }

}
