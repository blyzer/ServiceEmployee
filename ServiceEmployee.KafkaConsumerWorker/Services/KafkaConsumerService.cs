namespace ServiceEmployee.KafkaConsumerWorker.Services;

using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

public class KafkaConsumerService<TKey, TValue>
{
    private readonly string _bootstrapServers;
    private readonly string _topicName;
    private readonly string _groupId;

    public KafkaConsumerService(string bootstrapServers, string topicName, string groupId)
    {
        _bootstrapServers = bootstrapServers;
        _topicName = topicName;
        _groupId = groupId;
    }

    public void Consume(Action<ConsumeResult<TKey, TValue>> handleMessage, CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var consumer = new ConsumerBuilder<TKey, TValue>(config).Build())
        {
            consumer.Subscribe(_topicName);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    // Handle the message
                    handleMessage(consumeResult);
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}