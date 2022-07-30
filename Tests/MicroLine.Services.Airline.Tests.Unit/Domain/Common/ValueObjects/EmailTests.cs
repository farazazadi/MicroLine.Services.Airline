using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Common.ValueObjects;

public class EmailTests
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
    public void Email_ShouldThrowInvalidEmailException_WhenItCreatesFromNullOrEmptyInput(string email)
    {
        // Given
        // When
        var action = () => Email.Create(email);

        // Then
        action.Should().ThrowExactly<InvalidEmailException>()
            .And.Code.Should().Be(nameof(InvalidEmailException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void Email_ShouldThrowInvalidEmailException_WhenItConvertsFromNullOrEmptyInput(string email)
    {
        // Given
        // When
        var action = () => { var id = (Email)email; };

        // Then
        action.Should().ThrowExactly<InvalidEmailException>()
            .And.Code.Should().Be(nameof(InvalidEmailException));
    }



    public static TheoryData<string> InvalidEmails = new()
    {
        "aagmail.com",
        "123@g.c",
        "AF12345",
        "aa-b@yahoo",
        "mail@mailll",
        "http://www.mail@gmail.com",
        "http://mail@gmail.com",
    };

    [Theory, MemberData(nameof(InvalidEmails))]
    public void Email_ShouldThrowInvalidEmailException_WhenItCreatesFromInvalidEmailAddress(string email)
    {
        // Given
        // When
        var action = () => Email.Create(email);

        // Then
        action.Should().ThrowExactly<InvalidEmailException>()
            .And.Code.Should().Be(nameof(InvalidEmailException));
    }


    [Theory, MemberData(nameof(InvalidEmails))]
    public void Email_ShouldThrowInvalidEmailException_WhenItConvertsFromInvalidEmailAddress(string email)
    {
        // Given
        // When
        var action = () => { var id = (Email)email; };

        // Then
        action.Should().ThrowExactly<InvalidEmailException>()
            .And.Code.Should().Be(nameof(InvalidEmailException));
    }



    [Fact]
    public void Email_ShouldHaveValidToStringOutput_WhenItCreatedFromValidInput()
    {
        // Given
        var id = "test@test.com";

        // When
        var email = Email.Create(id);

        // Then
        email.ToString().Should().Be(id);
    }


    [Fact]
    public void Email_ShouldHaveEmptyToStringOutput_WhenItCreatesAsUnknown()
    {
        // Given
        // When
        var email = Email.Unknown;

        // Then
        email.ToString().Should().Be(string.Empty);

    }

}
