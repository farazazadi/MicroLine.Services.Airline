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

    private IConnection _connection;
    private IModel _channel;
    private string _exchangeName;
    private readonly string _routingKey = string.Empty;


    public RabbitMqClient(IOptions<RabbitMqOptions> rabbitMqOptions)
    {
        _rabbitMqOptions = rabbitMqOptions.Value;
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


    public Task<TEvent> GetAsync<TEvent>(Func<TEvent, bool> predicate,
        CancellationToken token = default)
    {
        var taskCompletionSource = new TaskCompletionSource<TEvent>();

        token.Register(() => taskCompletionSource.SetCanceled(token));

        var queue = _rabbitMqOptions.QueueToBind;

        Prepare();

        _channel.BasicQos(0, 1, false);

        var basicConsumer = new AsyncEventingBasicConsumer(_channel);

        var expectedEventName = typeof(TEvent).Name.Trim();

        basicConsumer.Received += (_, args) =>
        {
            if (args.BasicProperties.Type is null || args.BasicProperties.Type != expectedEventName)
            {
                _channel.BasicNack(args.DeliveryTag, multiple: false, requeue: false);
                return Task.CompletedTask;
            }

            var jsonMessage = Encoding.UTF8.GetString(args.Body.ToArray());

            var integrationEvent = JsonSerializer.Deserialize<TEvent>(jsonMessage);

            if (predicate(integrationEvent)) 
                taskCompletionSource.SetResult(integrationEvent);

            return Task.CompletedTask;
        };

        _channel.BasicConsume(queue, autoAck: true, consumer: basicConsumer);

        return taskCompletionSource.Task;
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
