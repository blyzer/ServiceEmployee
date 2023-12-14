namespace EmployeePermissions.Application.Queries;

using Confluent.Kafka;
using System;
using System.Threading.Tasks;

public class KafkaProducerService
{
    private readonly string _bootstrapServers;
    private readonly string _topicName;

    public KafkaProducerService(string bootstrapServers, string topicName)
    {
        _bootstrapServers = bootstrapServers;
        _topicName = topicName;
    }

    public async Task SendMessageAsync<TKey, TValue>(TKey key, TValue value)
    {
        var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

        using (var producer = new ProducerBuilder<TKey, TValue>(config).Build())
        {
            var message = new Message<TKey, TValue> { Key = key, Value = value };
            await producer.ProduceAsync(_topicName, message);
            
            // Wait for message to be delivered
            producer.Flush(TimeSpan.FromSeconds(5));
        }
    }
}