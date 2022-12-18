namespace MicroLine.Services.Airline.Infrastructure.Services.RabbitMq;
internal class RabbitMqOptions
{
    public static string SectionName => "RabbitMq";
    public string ClientProvidedName { get; set; }
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }
    public bool AutomaticRecoveryEnabled { get; set; }
    public string ExchangeName { get; set; }
    public string QueueToBind { get; set; }
    public byte RetryCountOnFailure { get; set; }
    public int BackOffFirstRetryDelayInSeconds { get; set; }
}