using MicroLine.Services.Airline.Domain.Common.Extensions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Infrastructure.Persistence.Entities;

namespace MicroLine.Services.Airline.Tests.Unit.Infrastructure.Persistence.Entities;

public class OutboxMessageTests
{

    public static TheoryData<string> NullOrEmptyStrings = new()
    {
        "",
        " ",
        "        ",
        string.Empty,
        null
    };



    [Fact]
    public void OutboxMessage_ShouldBeCreatedAsExpected_WhenInputIsValid()
    {
        // Given
        var id = Id.Create();
        var subject = "TestEvent";
        var content = "Test Content";


        // When
        var message = OutboxMessage.Create(id, subject, content);


        // Then
        message.SendStatus.Should().Be(OutboxMessage.Status.Scheduled);
        message.Subject.Should().Be(subject);
        message.Content.Should().Be(content);
        message.CreatedAtUtc.RemoveSecondsAndSmallerTimeUnites()
            .Should().Be(DateTime.UtcNow.RemoveSecondsAndSmallerTimeUnites());
    }


    [Fact]
    public void OutboxMessage_ShouldThrowArgumentException_WhenIdIsTransient()
    {
        // Given
        var id = Id.Transient;
        var subject = "TestEvent";
        var content = "Test Content";


        // When
        var func = ()=> OutboxMessage.Create(id, subject, content);


        // Then
        func.Should().ThrowExactly<ArgumentException>();
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void OutboxMessage_ShouldThrowArgumentException_WhenSubjectIsNotValid(string subject)
    {
        // Given
        var id = Id.Transient;
        var content = "Test Content";


        // When
        var func = () => OutboxMessage.Create(id, subject, content);


        // Then
        func.Should().ThrowExactly<ArgumentException>();
    }


    [Theory, MemberData(nameof(NullOrEmptyStrings))]
    public void OutboxMessage_ShouldThrowArgumentException_WhenContentIsNotValid(string content)
    {
        // Given
        var id = Id.Transient;
        var subject = "TestEvent";


        // When
        var func = () => OutboxMessage.Create(id, subject, content);


        // Then
        func.Should().ThrowExactly<ArgumentException>();
    }


    [Fact]
    public void OutboxMessage_ShouldHaveCorrectStatus_WhenSendMethodCalled()
    {
        // Given
        var id = Id.Create();
        var subject = "TestEvent";
        var content = "Test Content";


        // When
        var message = OutboxMessage.Create(id, subject, content);
        message.Send();

        // Then
        message.SendStatus.Should().Be(OutboxMessage.Status.Succeeded);
    }
}
