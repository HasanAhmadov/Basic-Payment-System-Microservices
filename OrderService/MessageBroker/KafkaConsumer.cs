using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OrderService.DTOs;
using OrderService.Exceptions;
using OrderService.Interfaces;
using System.Text.Json;

namespace OrderService.MessageBroker
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly KafkaConfiguration _kafkaConfig;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<KafkaConsumer> _logger;

        public KafkaConsumer(
            IOptions<KafkaConfiguration> kafkaConfig,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<KafkaConsumer> logger)
        {
            _kafkaConfig = kafkaConfig.Value;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Add delay to let app start first
            await Task.Delay(5000, stoppingToken);

            var config = new ConsumerConfig
            {
                GroupId = _kafkaConfig.GroupId,
                BootstrapServers = _kafkaConfig.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            try
            {
                consumer.Subscribe(_kafkaConfig.OrderTopic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(1));

                        if (consumeResult != null)
                        {
                            await ListenToPaymentEventAsync(consumeResult.Message.Value);
                            consumer.Commit(consumeResult);
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogWarning("Consume error: {Error}", ex.Error.Reason);
                        await Task.Delay(1000, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kafka consumer error");
            }
            finally
            {
                consumer?.Close();
            }
        }

        private async Task ListenToPaymentEventAsync(string paymentEventJson)
        {
            var paymentEventDTO = ConvertJsonToDTO(paymentEventJson);

            using var scope = _serviceScopeFactory.CreateScope();
            var messagingService = scope.ServiceProvider.GetRequiredService<IMessagingService>();

            await messagingService.ListenToPaymentEventAsync(paymentEventDTO);
        }

        private PaymentEventDTO ConvertJsonToDTO(string paymentEventJson)
        {
            try
            {
                return JsonSerializer.Deserialize<PaymentEventDTO>(paymentEventJson)
                    ?? throw new InvalidPaymentEventDTOInputException("Deserialized PaymentEventDTO is null");
            }
            catch (JsonException ex)
            {
                throw new InvalidPaymentEventDTOInputException("Invalid PaymentEventDTO input", ex);
            }
        }
    }
}