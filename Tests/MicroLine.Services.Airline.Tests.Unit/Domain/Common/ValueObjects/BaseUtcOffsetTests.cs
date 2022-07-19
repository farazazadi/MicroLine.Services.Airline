using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Common.ValueObjects;

public class BaseUtcOffsetTests
{
    public static TheoryData<int, int> InvalidBaseUtcOffsetList = new()
    {
        {-13, 0},
        {-10, 30},
        {15, 0},
        {7, 30},
        {14, 30},
        {0, 30},
        {3, -30},
        {-3, 30},
    };

    [Theory, MemberData(nameof(InvalidBaseUtcOffsetList))]
    public void BaseUtcOffset_ShouldThrowInvalidBaseUtcOffsetException_WhenItCreatesFromInvalidInput(int hours, int minutes)
    {
        // Given
        // When
        var action = () => BaseUtcOffset.Create(hours, minutes);

        // Then
        action.Should().ThrowExactly<InvalidBaseUtcOffsetException>()
            .And.Code.Should().Be(nameof(InvalidBaseUtcOffsetException));
    }


    [Fact]
    public void BaseUtcOffset_ShouldHaveCorrectToStringOutput_WhenItCreatesFromValidInput()
    {
        // Arrange
        var expected1 = $"UTC+03:30";
        var expected2 = $"UTC-03:30";
        // Act
        var offset1 = BaseUtcOffset.Create(3, 30);
        var offset2 = BaseUtcOffset.Create(-3, -30);

        // Assert
        offset1.ToString().Should().Be(expected1);
        offset2.ToString().Should().Be(expected2);
    }



    public static TheoryData<int, int> ValidBaseUtsOffsetList = new()
    {
        {-3, -30 },
        {3, 30 },
    };

    [Theory, MemberData(nameof(ValidBaseUtsOffsetList))]
    public void BaseUtcOffset_ShouldBeAbleToCastToTimeSpan(int hours, int minutes)
    {
        // Given
        var offset = BaseUtcOffset.Create(hours, minutes);

        // When
        TimeSpan timeSpan = offset;

        // Then
        timeSpan.Days.Should().Be(0);
        timeSpan.Hours.Should().Be(offset.Hours);
        timeSpan.Minutes.Should().Be(offset.Minutes);
        timeSpan.Seconds.Should().Be(0);
    }
}
