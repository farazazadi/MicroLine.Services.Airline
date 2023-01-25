using MicroLine.Services.Airline.Infrastructure.Extensions;
using MicroLine.Services.Airline.Infrastructure.Integration;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using RabbitMQ.Client;

namespace MicroLine.Services.Airline.Infrastructure.Services.RabbitMq;

internal class RabbitMqPublisher : IDisposable
{
    private readonly RabbitMqOptions _rabbitMqOptions;

    private IConnection _connection;
    private IModel _channel;
    private string _exchangeName;
    private readonly string _routingKey = string.Empty;

    private readonly RetryPolicy _retryPolicy;

    public RabbitMqPublisher(IOptions<RabbitMqOptions> rabbitMqOptions)
    {
        _rabbitMqOptions = rabbitMqOptions.Value;

        var delays = Backoff.DecorrelatedJitterBackoffV2(
            TimeSpan.FromSeconds(_rabbitMqOptions.BackOffFirstRetryDelayInSeconds),
            _rabbitMqOptions.RetryCountOnFailure);

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetry(delays);
    }

    private void CreateAndEstablishConnection()
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = _rabbitMqOptions.HostName,
            Port = _rabbitMqOptions.Port,
            UserName = _rabbitMqOptions.UserName,
            Password = _rabbitMqOptions.Password,
            VirtualHost = _rabbitMqOptions.VirtualHost,
            AutomaticRecoveryEnabled = _rabbitMqOptions.AutomaticRecoveryEnabled,
            DispatchConsumersAsync = true
        };


        _connection = connectionFactory.CreateConnection(_rabbitMqOptions.ClientProvidedName);
    }

    private void CreateChannel()
    {
        _channel = _connection.CreateModel();
    }

    private void DeclareExchange()
    {
        _exchangeName = _rabbitMqOptions.ExchangeName;

        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, true);
    }

    private void DeclareQueueAndBindToExchange()
    {
        var queue = _rabbitMqOptions.QueueToBind;

        _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queue, _exchangeName, _routingKey);
    }

    private void Prepare()
    {
        if (_channel is not null) return;

        CreateAndEstablishConnection();
        CreateChannel();
        DeclareExchange();
        DeclareQueueAndBindToExchange();
    }


    public void Publish<TIntegrationEvent>(TIntegrationEvent integrationEvent) where TIntegrationEvent : IntegrationEvent
    {
        var id = integrationEvent.EventId.ToString();
        var subject = typeof(TIntegrationEvent).Name;
        var content = integrationEvent.ToJsonString();

        Publish(id, subject, content);
    }

    public void Publish(string id, string subject, string content)
    {
        var contentBytes = content.ToByteArray();

        Publish(id, subject, contentBytes);
    }

    private void Publish(string id, string subject, byte[] content)
    {
        _retryPolicy.Execute(() =>
        {
            Prepare();

            var properties = _channel!.CreateBasicProperties();

            properties.MessageId = id;
            properties.Type = subject;

            _channel.BasicPublish(_exchangeName, _routingKey, body: content, basicProperties: properties);
        });
    }


    public void Dispose()
    {
        if (_channel is { IsOpen: true })
            _channel.Close();

        if (_connection is { IsOpen: true })
            _connection.Close();

        _channel?.Dispose();
        _connection?.Dispose();
    }
}
