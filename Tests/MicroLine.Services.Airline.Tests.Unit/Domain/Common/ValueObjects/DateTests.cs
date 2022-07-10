using FluentAssertions;
using MicroLine.Services.Airline.Domain.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Common.ValueObjects;

public class DateTests
{
    [Theory]
    [InlineData(2022, 13, 15)]
    [InlineData(2021, 12, 32)]
    [InlineData(2022, 11, 31)]
    [InlineData(31, 11, 2022)]
    [InlineData(33, 33, 3200)]
    public void Date_ShouldThrowInvalidDateException_WhenItCreatesFromInvalidInput(int year, int month, int day)
    {
        // Given
        // When
        var func = () => Date.Create(year, month, day);

        // Then
        func.Should().ThrowExactly<InvalidDateException>()
            .And.Code.Should().Be(nameof(InvalidDateException));
    }

    
    public static TheoryData<string> InvalidDateStrings = new()
    {
        "2022/13/15",
        "2021/12/32",
        "2022/11/31",
        "31/11/2022",
        "33/33/3200"
    };
    
    [Theory, MemberData(nameof(InvalidDateStrings))]
    public void Date_ShouldThrowInvalidDateException_WhenItCreatesFromInvalidString(string date)
    {
        // Given
        // When
        var func = () => Date.Create(date);

        // Then
        func.Should().ThrowExactly<InvalidDateException>()
            .And.Code.Should().Be(nameof(InvalidDateException));
    }

    [Theory, MemberData(nameof(InvalidDateStrings))]
    public void Date_ShouldThrowInvalidDateException_WhenItConvertsFromInvalidString(string date)
    {
        // Given
        // When
        var action = () => { Date implicitDate = date; };

        // Then
        action.Should().ThrowExactly<InvalidDateException>()
            .And.Code.Should().Be(nameof(InvalidDateException));
    }
    
    
    public static TheoryData<string> NullOrEmptyDateStrings = new()
    {
        "",
        " ",
        "        ",
        string.Empty,
        null
    };
    
    [Theory, MemberData(nameof(NullOrEmptyDateStrings))]
    public void Date_ShouldThrowInvalidDateException_WhenItCreatesFromNullOrEmptyString(string date)
    {
        // Given
        // When
        var action = () => { Date implicitDate = date; };

        // Then
        action.Should().ThrowExactly<InvalidDateException>()
            .And.Code.Should().Be(nameof(InvalidDateException));
    }

    [Theory, MemberData(nameof(NullOrEmptyDateStrings))]
    public void Date_ShouldThrowInvalidDateException_WhenItConvertsFromNullOrEmptyString(string date)
    {
        // Given
        // When
        var action = () => { Date implicitDate = date; };

        // Then
        action.Should().ThrowExactly<InvalidDateException>()
            .And.Code.Should().Be(nameof(InvalidDateException));
    }
    
    

    [Theory]
    [InlineData(2022, 10, 15)]
    [InlineData(2007, 10, 3)]
    [InlineData(2021, 1, 7)]
    [InlineData(2025, 11, 30)]
    [InlineData(2000, 2, 25)]
    public void Date_ShouldCreate_WhenCreatesFromValidInput(int year, int month, int day)
    {
        // Given
        // When
        var date = Date.Create(year, month, day);

        // Then
        date.Year.Should().Be(year);
        date.Month.Should().Be(month);
        date.Day.Should().Be(day);
    }
    
    
    public static TheoryData<string> ValidDateTimeStrings = new()
    {
        "2022/10/02",
        "2021/01/19",
        "2030/03/05",
        
        "10/02/2022",
        "01/19/2021",
        "03/05/2030",
        
        "7/9/2022 6:20:09 PM",
        "2021/01/19 6:20:09 PM"
    };

    [Theory, MemberData(nameof(ValidDateTimeStrings))]
    public void Date_ShouldCreate_WhenItCreatesFromValidDateString(string dateTimeString)
    {
        // Given
        var dateTime = DateTime.Parse(dateTimeString);

        // When
        var date = Date.Create(dateTimeString);

        // Then
        date.Year.Should().Be(dateTime.Year);
        date.Month.Should().Be(dateTime.Month);
        date.Day.Should().Be(dateTime.Day);
    }
    
    [Theory, MemberData(nameof(ValidDateTimeStrings))]
    public void Date_ShouldCreate_WhenItConvertsFromValidDateString(string dateTimeString)
    {
        // Given
        var dateTime = DateTime.Parse(dateTimeString);

        // When
        var date = (Date) dateTimeString;

        // Then
        date.Year.Should().Be(dateTime.Year);
        date.Month.Should().Be(dateTime.Month);
        date.Day.Should().Be(dateTime.Day);
    }
    

    public static TheoryData<DateTime> ValidDateTimes = new()
    {
            new DateTime(2022,10,15,5,14,20),
            new DateTime(2007,10,3,8, 15,52),
            new DateTime(2021,1,7),
            new DateTime(2025,11,30),
            new DateTime(2000,2,25)
    };

    [Theory, MemberData(nameof(ValidDateTimes))]
    public void Date_ShouldCreate_WhenItConvertsFromValidDateTimeObject(DateTime dateTime)
    {
        // Given
        // When
        var date = (Date)dateTime;

        // Then
        date.Year.Should().Be(dateTime.Year);
        date.Month.Should().Be(dateTime.Month);
        date.Day.Should().Be(dateTime.Day);
    }

    
    public static TheoryData<DateOnly> ValidDateOnlies = new()
    {
            new DateOnly(2022,10,15),
            new DateOnly(2007,10,15),
            new DateOnly(2021,1,7),
            new DateOnly(2025,11,30),
            new DateOnly(2000,2,25)
    };

    [Theory, MemberData(nameof(ValidDateOnlies))]
    public void Date_ShouldCreate_WhenItConvertsFromValidDateOnlyObject(DateOnly dateOnly)
    {
        // Given
        // When
        var date = (Date)dateOnly;

        // Then
        date.Year.Should().Be(dateOnly.Year);
        date.Month.Should().Be(dateOnly.Month);
        date.Day.Should().Be(dateOnly.Day);
    }

}
