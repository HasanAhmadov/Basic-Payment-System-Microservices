using Confluent.Kafka;
using PaymentService.DTOs;
using PaymentService.Exceptions;
using PaymentService.Interfaces;
using System.Text.Json;

namespace PaymentService.MessageBroker
{
    public class KafkaProducer : IKafkaProducer, IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topicName;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(IConfiguration configuration, ILogger<KafkaProducer> logger)
        {
            _logger = logger;
            _topicName = configuration["Kafka:PaymentTopic"] ?? "payment-events";

            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092",
                ClientId = "payment-service-producer"
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishPaymentEventAsync(PaymentEventDTO paymentEvent)
        {
            try
            {
                _logger.LogInformation("Publishing payment event for Order ID: {OrderId}", paymentEvent.OrderID);

                var json = JsonSerializer.Serialize(paymentEvent);
                var message = new Message<string, string>
                {
                    Key = paymentEvent.OrderID.ToString(),
                    Value = json
                };

                var result = await _producer.ProduceAsync(_topicName, message);
                _logger.LogInformation("Message published to topic {Topic}, partition {Partition}, offset {Offset}",
                    result.Topic, result.Partition, result.Offset);
            }
            catch (ProduceException<string, string> ex)
            {
                _logger.LogError(ex, "Error publishing message to Kafka");
                throw new PaymentEventDTOConversionException("Error while publishing payment event to Kafka", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error serializing PaymentEventDTO");
                throw new PaymentEventDTOConversionException("Error while converting PaymentEventDTO to JSON", ex);
            }
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}