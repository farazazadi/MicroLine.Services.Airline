using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using static MicroLine.Services.Airline.Domain.Common.ValueObjects.Speed;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Common.ValueObjects;

public class SpeedTests
{

    [Theory]
    [InlineData(-2, UnitOfSpeed.Knot)]
    [InlineData(-5, UnitOfSpeed.KilometresPerHour)]
    [InlineData(-1000, UnitOfSpeed.MilesPerHour)]
    public void Speed_ShouldThrowInvalidSpeedException_WhenItCreatesFromNegativeValue(int speed, UnitOfSpeed unit)
    {
        // Given
        // When
        var func = () => Speed.Create(speed, unit);

        // Then
        func.Should().ThrowExactly<InvalidSpeedException>()
            .And.Code.Should().Be(nameof(InvalidSpeedException));
    }


    [Theory]
    [InlineData("-500 Km/h")]
    [InlineData("-5 mph")]
    [InlineData("-320 Kn")]
    public void Speed_ShouldThrowInvalidSpeedException_WhenItConvertsFromStringWithNegativeValue(string speed)
    {
        // Given
        // When
        var action = () => { Speed implicitSpeed = speed; };

        // Then
        action.Should().ThrowExactly<InvalidSpeedException>()
            .And.Code.Should().Be(nameof(InvalidSpeedException));
    }

    
    [Theory]
    [InlineData("4 ph")]
    [InlineData("500 k")]
    [InlineData("1000  ")]
    [InlineData("1000  4")]
    public void Speed_ShouldThrowInvalidSpeedException_WhenItConvertsFromStringWithInvalidUnit(string speed)
    {
        // Given
        // When
        var action = () => { Speed implicitSpeed = speed; };

        // Then
        action.Should().ThrowExactly<InvalidSpeedException>()
            .And.Code.Should().Be(nameof(InvalidSpeedException));
    }
    
    public static TheoryData<string> NullOrEmptyStrings = new()
    {
        "",
        " ",
        "        ",
        string.Empty,
        null
    };
    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void Speed_ShouldThrowInvalidSpeedException_WhenItConvertsFromStringWithNullOrEmptyString(string speed)
    {
        // Given
        // When
        var action = () => { Speed implicitSpeed = speed; };

        // Then
        action.Should().ThrowExactly<InvalidSpeedException>()
            .And.Code.Should().Be(nameof(InvalidSpeedException));
    }

    
    

    [Theory]
    [InlineData(700, UnitOfSpeed.KilometresPerHour)]
    [InlineData(600, UnitOfSpeed.MilesPerHour)]
    [InlineData(280, UnitOfSpeed.Knot)]
    public void Speed_ShouldCreate_WhenItCreatesFromValidInput(int speed, UnitOfSpeed unit)
    {
        // Given
        // When
        var speedObj = Create(speed, unit);

        // Then
        speedObj.Value.Should().Be(speed);
        speedObj.Unit.Should().Be(unit);
    }


    [Theory]
    [InlineData("700 Km/h")]
    [InlineData("600 mph")]
    [InlineData("280 Kn")]
    public void Speed_ShouldCreate_WhenItConvertsFromValidInput(string speed)
    {
        // Given
            Dictionary<string, UnitOfSpeed> unitOfSpeedSymbols = new Dictionary<string, UnitOfSpeed>
            {
                { "Km/h", UnitOfSpeed.KilometresPerHour},
                { "mph", UnitOfSpeed.MilesPerHour},
                { "Kn",  UnitOfSpeed.Knot}
            };

            var speedParts = speed.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // When
        var speedObj = (Speed)speed;

        // Then
        speedObj.Value.Should().Be(int.Parse(speedParts[0]));
        speedObj.Unit.Should().Be(unitOfSpeedSymbols[speedParts[1]]);
    }


    [Fact]
    public void ConvertToMethod_ShouldReturnExpectedValue_WhenItCalled()
    {
        // Given
        var speedInKilometers = Speed.Create(850, UnitOfSpeed.KilometresPerHour);
        var speedInMiles = Speed.Create(528, UnitOfSpeed.MilesPerHour);
        var speedInKnot = Speed.Create(459, UnitOfSpeed.Knot);

        // When
        // Then
        speedInKilometers.ConvertTo(UnitOfSpeed.KilometresPerHour)
            .Should().Be(speedInKilometers.Value);

        speedInKilometers.ConvertTo(UnitOfSpeed.MilesPerHour)
            .Should().Be(speedInMiles.Value);

        speedInKilometers.ConvertTo(UnitOfSpeed.Knot)
            .Should().Be(speedInKnot.Value);


        speedInMiles.ConvertTo(UnitOfSpeed.KilometresPerHour)
            .Should().Be(speedInKilometers.Value);

        speedInMiles.ConvertTo(UnitOfSpeed.MilesPerHour)
            .Should().Be(speedInMiles.Value);

        speedInMiles.ConvertTo(UnitOfSpeed.Knot)
            .Should().Be(speedInKnot.Value);



        speedInKnot.ConvertTo(UnitOfSpeed.KilometresPerHour)
            .Should().Be(speedInKilometers.Value);

        speedInKnot.ConvertTo(UnitOfSpeed.MilesPerHour)
            .Should().Be(speedInMiles.Value);

        speedInKnot.ConvertTo(UnitOfSpeed.Knot)
            .Should().Be(speedInKnot.Value);

    }
}
