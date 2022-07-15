using FluentAssertions;
using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Common.ValueObjects;

public class FullNameTests
{
    public static TheoryData<string, string> NullOrEmptyFullNames = new ()
    {
        {"Faraz", " " },
        {" ", "Azadi" },
        {" ", " " },
        {string.Empty, " " },
        {null , " " },
        {" ", string.Empty },
        {" ", null },
        {string.Empty, null },
        {null, string.Empty},
        {string.Empty, string.Empty},
        {null, null }
    };

    [Theory, MemberData(nameof(NullOrEmptyFullNames))]
    public void FullName_ShouldThrowInvalidFullNameException_WhenItCreatesFromNullOrEmptyInput(string firstName, string lastName)
    {
        // Given
        // When
        var action = () => FullName.Create(firstName, lastName);

        // Then
        action.Should().ThrowExactly<InvalidFullNameException>()
            .And.Code.Should().Be(nameof(InvalidFullNameException));

    }


    public static TheoryData<string, string> FirstAndLastNamesWithLengthLessThan3OrGreaterThan50 = new()
    {
        {"Fa", "Azadi" },
        {"Fa ", "Azadi" },
        {" F", "Azadiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii" },
        {"Farazzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz", " Az" },
        {"Farazzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz", " Azadi" },
        {"Farazzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz", " Azadiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii" },
        {"Fa", "Az" },

    };

    [Theory, MemberData(nameof(FirstAndLastNamesWithLengthLessThan3OrGreaterThan50))]
    public void FullName_ShouldThrowInvalidFullNameException_WhenItCreatesFromFirstNameOrLastNameWithLengthLessThan3OrGreaterThan50(string firstName, string lastName)
    {
        // Given
        // When
        var action = () => FullName.Create(firstName, lastName);

        // Then
        action.Should().ThrowExactly<InvalidFullNameException>()
            .And.Code.Should().Be(nameof(InvalidFullNameException));
    }


    public static TheoryData<string, string> FirstAndLastNamesWithIllegalCharacters = new()
    {
        {"Faraz!", "Azadi" },
        {"Faraz2", "Azadi" },
        {"Faraz", "Azadi#" },
        {"Faraz", "Azadi8" }
    };

    [Theory, MemberData(nameof(FirstAndLastNamesWithIllegalCharacters))]
    public void FullName_ShouldThrowInvalidFullNameException_WhenItCreatesFromFirstNameOrLastNameWithIllegalCharacters(string firstName, string lastName)
    {
        // Given
        // When
        var action = () => FullName.Create(firstName, lastName);

        // Then
        action.Should().ThrowExactly<InvalidFullNameException>()
            .And.Code.Should().Be(nameof(InvalidFullNameException));
    }

    [Fact]
    public void FullName_ShouldHaveSpaceSeparatedToStringOutput_WhenItCreatesFromValidInput()
    {
        // Given
        var firstName = "Faraz";
        var lastName = "Azadi";

        // When
        var fullName = FullName.Create(firstName, lastName);

        // Then
        fullName.ToString().Should().Be($"{firstName} {lastName}");
    }

}
