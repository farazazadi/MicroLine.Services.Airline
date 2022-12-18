namespace MicroLine.Services.Airline.Infrastructure.Services.Outbox;

internal class OutboxProcessorOptions
{
    public static string SectionName => "OutboxProcessor";
    public int ProcessingIntervalInSeconds { get; set; }
    public int AllowedExceptionsCountBeforeBreaking { get; set; }
    public int DurationOfBreakInSeconds { get; set; }
}