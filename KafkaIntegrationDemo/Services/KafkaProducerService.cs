using Confluent.Kafka;
using KafkaIntegrationProducer.Models;
using Microsoft.Extensions.Options;

namespace KafkaIntegrationProducer.Services
{
    public class KafkaProducerService
    {
        private readonly string _topic;
        private readonly IProducer<Null, string> _producer;

        public KafkaProducerService(IOptions<KafkaSettings> kafkaSettings)
        {
            var config = new ProducerConfig { BootstrapServers = kafkaSettings.Value.BootstrapServers };
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _topic = kafkaSettings.Value.Topic;
        }

        public async Task SendMessageAsync(string message)
        {
            var result = await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
            Console.WriteLine($"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
        }
    }
}
