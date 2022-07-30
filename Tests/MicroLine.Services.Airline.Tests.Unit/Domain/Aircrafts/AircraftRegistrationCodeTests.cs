using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Aircrafts.Exceptions;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Aircrafts;
public class AircraftRegistrationCodeTests
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
    public void AircraftRegistrationCode_ShouldThrowInvalidAircraftRegistrationCodeException_WhenItCreatedFromNullOrEmptyString(string registrationCode)
    {
        // Given
        // When
        var func = () => AircraftRegistrationCode.Create(registrationCode);

        // Then
        func.Should().ThrowExactly<InvalidAircraftRegistrationCodeException>()
            .And.Code.Should().Be(nameof(InvalidAircraftRegistrationCodeException));
    }

    
    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void AircraftRegistrationCode_ShouldThrowInvalidAircraftRegistrationCodeException_WhenItConvertsFromNullOrEmptyString(string registrationCode)
    {
        // Given
        // When
        var action = () => { AircraftRegistrationCode implicitAircraftRegistrationCode = registrationCode; };
    
        // Then
    
        action.Should().ThrowExactly<InvalidAircraftRegistrationCodeException>()
            .And.Code.Should().Be(nameof(InvalidAircraftRegistrationCodeException));
    }
    

    
    public static TheoryData<string> StringsWithLengthLessThan3OrGreaterThan20 = new()
    {
        "Ar",
        "a",
        "AC             ",
        "           SA",
        " A9  ",
        "     EP-FSAFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD       ",
        "1"
    };
    
    [Theory, MemberData(nameof(StringsWithLengthLessThan3OrGreaterThan20))]
    public void AircraftRegistrationCode_ShouldThrowInvalidAircraftRegistrationCodeException_WhenItCreatesFromStringWithLengthLessThan3OrGreaterThan20(string registrationCode)
    {
        // Given
        // When
        var func = () => AircraftRegistrationCode.Create(registrationCode);

        // Then
        func.Should().ThrowExactly<InvalidAircraftRegistrationCodeException>()
            .And.Code.Should().Be(nameof(InvalidAircraftRegistrationCodeException));
    }

    
    [Theory, MemberData(nameof(StringsWithLengthLessThan3OrGreaterThan20))]
    public void AircraftRegistrationCode_ShouldThrowInvalidAircraftRegistrationCodeException_WhenItConvertsFromStringWithLengthLessThan3OrGreaterThan20(string registrationCode)
    {
        // Given
        // When
        var action = () => { AircraftRegistrationCode implicitRegistrationCode = registrationCode; };
    
        // Then
        action.Should().ThrowExactly<InvalidAircraftRegistrationCodeException>()
            .And.Code.Should().Be(nameof(InvalidAircraftRegistrationCodeException));
    }


    public static TheoryData<string> ValidRegistrationCodeStrings = new()
    {
        "AF35",
        "  Ep-8744",
        "fA-632  ",
        "8963",
        "   EPA2  ",
        "     EP-FSAF       ",
        "AA-523"
    };
    
    [Theory, MemberData(nameof(ValidRegistrationCodeStrings))]
    public void AircraftRegistrationCode_ShouldBeCreated_WhenItCreatesFromValidString(string registrationCode)
    {
        // Given
        // When
        var code = AircraftRegistrationCode.Create(registrationCode);

        // Then
        code.ToString().Should().Be(registrationCode.Trim());
    }
    
    
    [Theory, MemberData(nameof(ValidRegistrationCodeStrings))]
    public void AircraftRegistrationCode_ShouldBeCreated_WhenItConvertsFromValidString(string registrationCode)
    {
        // Given
        // When
        var code = (AircraftRegistrationCode)registrationCode;
    
        // Then
        code.ToString().Should().Be(registrationCode.Trim());
    }
    
}
