using MicroLine.Services.Airline.Infrastructure.Persistence;
using MicroLine.Services.Airline.Infrastructure.Persistence.Entities;
using MicroLine.Services.Airline.Infrastructure.Services.RabbitMq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;

namespace MicroLine.Services.Airline.Infrastructure.Services.Outbox;

internal sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly RabbitMqPublisher _rabbitMqPublisher;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly PeriodicTimer _periodicTimer;

    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

    public OutboxProcessor(
        IOptions<OutboxProcessorOptions> options,
        IServiceScopeFactory serviceScopeFactory,
        RabbitMqPublisher rabbitMqPublisher,
        ILogger<OutboxProcessor> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _rabbitMqPublisher = rabbitMqPublisher;
        _logger = logger;

        var option = options.Value;

        _circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(option.AllowedExceptionsCountBeforeBreaking,
                TimeSpan.FromSeconds(option.DurationOfBreakInSeconds));

        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(option.ProcessingIntervalInSeconds));
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        
        while (!token.IsCancellationRequested &&
               await _periodicTimer.WaitForNextTickAsync(token))
        {
            if (_circuitBreakerPolicy.CircuitState is CircuitState.Open or CircuitState.Isolated)
                continue;

            try
            {
                await _circuitBreakerPolicy.ExecuteAsync(async () => await ProcessAsync(token));
            }
            catch
            {
                // ignored
            }
        }
    }

    public async Task ProcessAsync(CancellationToken token)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();

        var airlineDbContext = scope.ServiceProvider.GetRequiredService<AirlineDbContext>();


        var outboxMessages = await airlineDbContext.OutboxMessages
            .Where(message => message.SendStatus == OutboxMessage.Status.Scheduled)
            .ToListAsync(token);

        if(!outboxMessages.Any())
            return;

        var executionStrategy = airlineDbContext.Database.CreateExecutionStrategy();


        foreach (var message in outboxMessages)
        {
            try
            {
                await executionStrategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await airlineDbContext.Database.BeginTransactionAsync(token);

                    _rabbitMqPublisher.Publish(message.Id.ToString(), message.Subject, message.Content);

                    message.Send();

                    await airlineDbContext.SaveChangesAsync(token);

                    await transaction.CommitAsync(token);

                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "The message with Id '{MessageId}' and '{MessageSubject}' could not be published!",
                    message.Id, message.Subject);

                throw;
            }
        }

    }

    public override void Dispose()
    {
        base.Dispose();
        _rabbitMqPublisher.Dispose();
    }
}
