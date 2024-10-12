using Confluent.Kafka;
using KafkaIntegrationProducer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Confluent.Kafka.ConfigPropertyNames;

namespace KafkaIntegrationProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaMsgController : ControllerBase
    {

        private readonly ILogger<KafkaMsgController> _logger;
        private readonly KafkaProducerService _kafkaProducerService;

        public KafkaMsgController(ILogger<KafkaMsgController> logger, KafkaProducerService kafkaProducerService)
        {
            _logger = logger;
            _kafkaProducerService = kafkaProducerService;
        }

        [HttpPost]
        public async Task<IActionResult> Send(string message)
        {
            await _kafkaProducerService.SendMessageAsync(message);

            return Ok("Message sent to Kafka");
        }

    }
}
