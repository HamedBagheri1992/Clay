using ClayService.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class Worker : IHostedService
{
    private readonly ILogger<Worker> _logger;
    private readonly IKafkaConsumer _kafkaConsumer;

    public Worker(ILogger<Worker> logger, IKafkaConsumer kafkaConsumer)
    {
        _logger = logger;
        _kafkaConsumer = kafkaConsumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        if (_kafkaConsumer.Init())
        {
            _kafkaConsumer.Start();
        }
        else
            _logger.LogWarning("Consumer could not Start...");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _kafkaConsumer.Stop();
        _logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);

        return Task.CompletedTask;
    }
}