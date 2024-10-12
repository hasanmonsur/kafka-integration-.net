using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace KafkaIntegrationConsumer
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<KafkaConsumerService> _logger;

        public KafkaConsumerService(IConfiguration configuration, ILogger<KafkaConsumerService> logger)
        {
            _logger = logger;
            var KafkaUrl = "127.0.0.1:9092";
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = KafkaUrl,
                GroupId = "InventoryConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string kafkTopic = "test-topic";
            _consumer.Subscribe(kafkTopic);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() => ProcessKafkaMessage(stoppingToken), stoppingToken);
                await Task.Delay(10, stoppingToken);
            }

            _consumer.Close();
        }

        public void ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                Console.WriteLine($"Consume Message : {message}");

                _logger.LogInformation(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex+ "Error processing Kafka message");
                _logger.LogError(ex, "Error processing Kafka message");
            }
        }
    }
}
