using System.Text;
using System.Text.Json;
using MicroLine.Services.Airline.Infrastructure.Services.RabbitMq;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroLine.Services.Airline.Tests.Integration.Common;

internal class RabbitMqClient : IDisposable
{
    private readonly RabbitMqOptions _rabbitMqOptions;

    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly string _routingKey = string.Empty;

    private static readonly SynchronizedList<(string Subject, string Content)> ReceivedMessages = new();
    private static readonly SynchronizedList<(Type type, dynamic message)> DeserializedMessages = new();

    public RabbitMqClient(IOptions<RabbitMqOptions> rabbitMqOptions)
    {
        _rabbitMqOptions = rabbitMqOptions.Value;


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

        _channel = _connection.CreateModel();

        var exchangeName = _rabbitMqOptions.ExchangeName;
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true);


        _queueName = _rabbitMqOptions.QueueToBind;
        _channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(_queueName, exchangeName, _routingKey);

        Subscribe();
    }



    public void Subscribe()
    {
        _channel.BasicQos(0, 1, false);
        var basicConsumer = new AsyncEventingBasicConsumer(_channel);

        basicConsumer.Received += (_, args) =>
        {
            var messageType = args.BasicProperties.Type;

            if (messageType is null)
                return Task.CompletedTask;


            var messageContent = Encoding.UTF8.GetString(args.Body.ToArray());

            ReceivedMessages.Add((messageType, messageContent));

            return Task.CompletedTask;
        };

        var queue = _rabbitMqOptions.QueueToBind;

        _channel.BasicConsume(queue, autoAck: true, consumer: basicConsumer);

    }


    public TMessage GetMessage<TMessage>(Func<TMessage, bool> predicate,
        CancellationToken token = default)
    {
        while (!token.IsCancellationRequested)
        {
            var eventType = typeof(TMessage);

            var deserializedMessage = DeserializedMessages
                .SingleOrDefault(m => m.type == eventType && predicate(m.message));

            if (deserializedMessage.message is not null)
            {
                DeserializedMessages.Remove(deserializedMessage);

                return deserializedMessage.message;
            }


            var typeName = eventType.Name.Trim();

            var receivedMessagesWithExpectedType = ReceivedMessages
                .Where(m => m.Subject == typeName)
                .ToList();

            foreach (var receivedMessage in receivedMessagesWithExpectedType)
            {
                ReceivedMessages.Remove(receivedMessage);

                var message = JsonSerializer.Deserialize<TMessage>(receivedMessage.Content);

                if (predicate(message))
                    return message;

                DeserializedMessages.Add((eventType, message));
            }
        }

        return default;
    }

    public void Dispose()
    {
        if (_channel is { IsOpen: true })
        {
            _channel.Close();
            _channel.Dispose();
        }

        if (_connection is { IsOpen: true })
        {
            _connection.Close();
            _connection?.Dispose();
        }
    }
}