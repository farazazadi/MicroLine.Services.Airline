using FluentAssertions;
using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Common.ValueObjects;

public class AddressTests
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
    public void Address_ShouldThrowInvalidAddressException_WhenItCreatesAndStreetIsNullOrEmpty(string street)
    {
        // Given
        // When
        var action = () => Address.Create(street, "Toronto", "Ontario", "Canada","123");

        // Then
        action.Should().ThrowExactly<InvalidAddressException>()
            .And.Code.Should().Be(nameof(InvalidAddressException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void Address_ShouldThrowInvalidAddressException_WhenItCreatesAndCityIsNullOrEmpty(string city)
    {
        // Given
        // When
        var action = () => Address.Create("154 Maple Ave", city, "Ontario", "Canada", "123");

        // Then
        action.Should().ThrowExactly<InvalidAddressException>()
            .And.Code.Should().Be(nameof(InvalidAddressException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void Address_ShouldThrowInvalidAddressException_WhenItCreatesAndStateIsNullOrEmpty(string state)
    {
        // Given
        // When
        var action = () => Address.Create("154 Maple Ave", "Toronto", state, "Canada", "123");

        // Then
        action.Should().ThrowExactly<InvalidAddressException>()
            .And.Code.Should().Be(nameof(InvalidAddressException));
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void Address_ShouldThrowInvalidAddressException_WhenItCreatesAndCountryIsNullOrEmpty(string country)
    {
        // Given
        // When
        var action = () => Address.Create("154 Maple Ave", "Toronto", "Ontario", country, "123");

        // Then
        action.Should().ThrowExactly<InvalidAddressException>()
            .And.Code.Should().Be(nameof(InvalidAddressException));
    }



    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void Address_ShouldThrowInvalidAddressException_WhenItCreatesAndPostalCodeIsNullOrEmpty(string postalCode)
    {
        // Given
        // When
        var action = () => Address.Create("154 Maple Ave", "Toronto", "Ontario", "Canada", postalCode);

        // Then
        action.Should().ThrowExactly<InvalidAddressException>()
            .And.Code.Should().Be(nameof(InvalidAddressException));
    }





    [Fact]
    public void Address_ShouldHaveValidToStringOutput_WhenItCreatedFromValidInput()
    {
        // Given
        var street = "154 Maple Ave";
        var city = "Toronto";
        var state = "Ontario";
        var country = "Canada";
        var postalCode = "123";

        var expected = $"{street}, {city}, {state}, {country}, {postalCode}";

        // When
        var address = Address.Create(street, city, state, country, postalCode);

        // Then
        address.ToString().Should().Be(expected);

    }


    [Fact]
    public void Address_ShouldHaveEmptyToStringOutput_WhenItCreatesAsUnknown()
    {
        // Given
        // When
        var address = Address.Unknown;

        // Then
        address.ToString().Should().Be(string.Empty);

    }

}