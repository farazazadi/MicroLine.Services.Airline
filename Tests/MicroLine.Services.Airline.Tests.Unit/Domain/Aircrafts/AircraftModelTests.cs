using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Aircrafts.Exceptions;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Aircrafts;

public class AircraftModelTests
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
    public void AircraftModel_ShouldThrowInvalidAircraftModelException_WhenItCreatesFromNullOrEmptyString(string aircraftModel)
    {
        // Given
        // When
        var func = () => AircraftModel.Create(aircraftModel);

        // Then
        func.Should().ThrowExactly<InvalidAircraftModelException>()
            .And.Code.Should().Be(nameof(InvalidAircraftModelException));
    }
    
    
    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void AircraftModel_ShouldThrowInvalidAircraftModelException_WhenItConvertsFromNullOrEmptyString(string aircraftModel)
    {
        // Given
        // When
        var createByImplicitConversationFromString = () => { AircraftModel model = aircraftModel; };
    
        // Then
        createByImplicitConversationFromString.Should().ThrowExactly<InvalidAircraftModelException>()
            .And.Code.Should().Be(nameof(InvalidAircraftModelException));
    }



    public static TheoryData<string> AircraftModelStringsWithLengthLessThan3OrGreaterThan15 = new()
    {
        "32",
        "a",
        "a             ",
        "           a",
        " a3  ",
        "     a3200000000000000000000       ",
        "1"
    };

    [Theory, MemberData(nameof(AircraftModelStringsWithLengthLessThan3OrGreaterThan15))]
    public void AircraftModel_ShouldThrowInvalidAircraftModelException_WhenItCreatesFromStringWithLengthLessThan3OrGreaterThan15(string aircraftModel)
    {
        // Given
        // When
        var func = () => AircraftModel.Create(aircraftModel);

        // Then
        func.Should().ThrowExactly<InvalidAircraftModelException>()
            .And.Code.Should().Be(nameof(InvalidAircraftModelException));
    }
    
    [Theory, MemberData(nameof(AircraftModelStringsWithLengthLessThan3OrGreaterThan15))]
    public void AircraftModel_ShouldThrowInvalidAircraftModelException_WhenItConvertsFromStringWithLengthLessThan3OrGreaterThan15(string aircraftModel)
    {
        // Given
        // When
        var action = () => { AircraftModel model = aircraftModel; };
    
        // Then
        action.Should().ThrowExactly<InvalidAircraftModelException>()
            .And.Code.Should().Be(nameof(InvalidAircraftModelException));
    }


    public static TheoryData<string> ValidAircraftModelStrings = new()
    
    {
        "A320",
        "  A320",
        "A320  ",
        "737",
        "   DC10  ",
        "     737Max       ",
        "A350"
    };
    [Theory, MemberData(nameof(ValidAircraftModelStrings))]
    public void AircraftModel_ShouldBeCreated_WhenItCreatesFromValidString(string aircraftModel)
    {
        // Given
        // When
        var model = AircraftModel.Create(aircraftModel);

        // Then
        model.ToString().Should().Be(aircraftModel.Trim());
    }
    
    [Theory, MemberData(nameof(ValidAircraftModelStrings))]
    public void AircraftModel_ShouldBeCreated_WhenItConvertsFromValidString(string aircraftModel)
    {
        // Given
        // When
        var model =  (AircraftModel)aircraftModel;
    
        // Then
        model.ToString().Should().Be(aircraftModel.Trim());
    }

    
}