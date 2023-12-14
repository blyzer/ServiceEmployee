using Confluent.Kafka;
using ServiceEmployee.KafkaConsumerWorker.Services;

namespace ServiceEmployee.KafkaConsumerWorker;

using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class KafkaConsumerHostedService(
    KafkaConsumerService<string, string> kafkaConsumerService,
    ILogger<KafkaConsumerHostedService> logger)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        kafkaConsumerService.Consume(result =>
        {
            logger.LogInformation($"Received message: {result.Message.Value}");
        }, stoppingToken);

        return Task.CompletedTask;
    }
    
    private void ProcessMessage(ConsumeResult<string, string> consumeResult)
    {
        try
        {
            // Implement your logic to handle the message.
            // For example:
            string key = consumeResult.Message.Key;
            string value = consumeResult.Message.Value;
            logger.LogInformation($"Received message: Key: {key}, Value: {value}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing Kafka message.");
        }
    }
}