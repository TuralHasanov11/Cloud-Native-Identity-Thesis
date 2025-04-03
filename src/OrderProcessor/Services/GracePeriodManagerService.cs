using MassTransit;
using Microsoft.Extensions.Options;
using Npgsql;
using OrderProcessor.Events;

namespace OrderProcessor.Services;

public class GracePeriodManagerService(
    IOptions<BackgroundTaskOptions> options,
    IPublishEndpoint eventBus,
    ILogger<GracePeriodManagerService> logger,
    IConfiguration configuration)
    : BackgroundService
{
    private readonly BackgroundTaskOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delayTime = TimeSpan.FromSeconds(_options.CheckUpdateTime);

        if (logger.IsEnabled(LogLevel.Debug))
        {
            stoppingToken.Register(logger.LogStopping);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogDoingBackgroundWork();

            await CheckConfirmedGracePeriodOrders();

            await Task.Delay(delayTime, stoppingToken);
        }

        logger.LogStarting();
    }

    private async Task CheckConfirmedGracePeriodOrders()
    {
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogCheckingOrders();
        }

        var orderIds = await GetConfirmedGracePeriodOrders();

        foreach (var orderId in orderIds)
        {
            var confirmGracePeriodEvent = new GracePeriodConfirmedIntegrationEvent(orderId);

            logger.LogPublishingIntegrationEvent(confirmGracePeriodEvent.Id, confirmGracePeriodEvent);

            await eventBus.Publish(confirmGracePeriodEvent);
        }
    }

    private async ValueTask<List<Guid>> GetConfirmedGracePeriodOrders()
    {
        try
        {
            await using var dataSource = NpgsqlDataSource.Create(configuration.GetConnectionString("Database")!);

            await using var conn = dataSource.CreateConnection();
            await using var command = conn.CreateCommand();
            command.CommandText = """
                    SELECT "Id"
                    FROM ordering.orders
                    WHERE CURRENT_TIMESTAMP - "OrderDate" >= @GracePeriodTime AND "OrderStatus" = 'Submitted'
                    """;
            command.Parameters.AddWithValue("GracePeriodTime", TimeSpan.FromMinutes(_options.GracePeriodTime));

            List<Guid> ids = [];

            await conn.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ids.Add(reader.GetGuid(0));
            }

            return ids;
        }
        catch (NpgsqlException exception)
        {
            logger.LogDataSourceError(exception);
        }

        return [];
    }
}

public static partial class GracePeriodManagerServiceLogger
{
    [LoggerMessage(LogLevel.Debug, "GracePeriodManagerService is starting.")]
    public static partial void LogStarting(this ILogger<GracePeriodManagerService> logger);

    [LoggerMessage(LogLevel.Debug, "GracePeriodManagerService background task is stopping.")]
    public static partial void LogStopping(this ILogger<GracePeriodManagerService> logger);

    [LoggerMessage(LogLevel.Debug, "GracePeriodManagerService background task is doing background work.")]
    public static partial void LogDoingBackgroundWork(this ILogger<GracePeriodManagerService> logger);

    [LoggerMessage(LogLevel.Information, "Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})")]
    public static partial void LogPublishingIntegrationEvent(
        this ILogger<GracePeriodManagerService> logger,
        Guid integrationEventId,
        IntegrationEvent integrationEvent);

    [LoggerMessage(LogLevel.Error, "Fatal error establishing database connection: {Exception}")]
    public static partial void LogDataSourceError(
        this ILogger<GracePeriodManagerService> logger,
        Exception exception);

    [LoggerMessage(LogLevel.Debug, "Checking confirmed grace period orders")]
    public static partial void LogCheckingOrders(this ILogger<GracePeriodManagerService> logger);
}
