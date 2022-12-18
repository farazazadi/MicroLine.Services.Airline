using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Entities;

internal class OutboxMessage : Entity
{
    public enum Status : byte
    {
        Scheduled = 0,
        Succeeded = 1
    }

    public string Subject { get; init; }

    public string Content { get; init; }

    public Status SendStatus { get; private set; }

    public DateTime CreatedAtUtc { get; init; }

    private OutboxMessage()
    {}

    private OutboxMessage(Id id, string subject, string content, Status sendStatus, DateTime createdAtUtc)
    {
        Id = id;
        Subject = subject;
        Content = content;
        SendStatus = sendStatus;
        CreatedAtUtc = createdAtUtc;
    }

    public static OutboxMessage Create (Id id, string subject, string content)
    {
        if (id.IsTransient)
            throw new ArgumentException($"The {nameof(Id)} of message is not valid!");

        if(string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException($"The {nameof(Subject)} of message can not be null or empty!");

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException($"The {nameof(Content)} of message can not be null or empty!");


        return new OutboxMessage(id, subject, content, Status.Scheduled, DateTime.UtcNow);
    }

    public void Send()
    {
        SendStatus = Status.Succeeded;
    }
}
