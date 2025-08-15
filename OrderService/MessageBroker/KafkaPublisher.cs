using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OrderService.DTOs;
using OrderService.Exceptions;
using OrderService.Interfaces;
using System.Text.Json;

namespace OrderService.MessageBroker
{
    public class KafkaPublisher : IKafkaPublisher
    {
        private readonly KafkaConfiguration _kafkaConfig;
        private readonly ILogger<KafkaPublisher> _logger;
        private readonly IProducer<string, string> _producer;

        public KafkaPublisher(IOptions<KafkaConfiguration> kafkaConfig, ILogger<KafkaPublisher> logger)
        {
            _kafkaConfig = kafkaConfig.Value;
            _logger = logger;

            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaConfig.BootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishMessageAsync(PaymentEventDTO paymentEventDTO)
        {
            _logger.LogInformation("Publishing message to topic {Topic}", _kafkaConfig.PaymentTopic);

            try
            {
                var paymentEventJson = JsonSerializer.Serialize(paymentEventDTO);

                var message = new Message<string, string>
                {
                    Key = paymentEventDTO.OrderID.ToString(),
                    Value = paymentEventJson
                };

                await _producer.ProduceAsync(_kafkaConfig.PaymentTopic, message);
            }
            catch (Exception ex)
            {
                throw new PaymentEventDTOConversionException("Error while publishing PaymentEventDTO to Kafka", ex);
            }
        }

        public void Dispose()
        {
            _producer?.Dispose();
        }
    }
}