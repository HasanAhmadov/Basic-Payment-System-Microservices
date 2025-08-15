using Confluent.Kafka;
using PaymentService.DTOs;
using PaymentService.Exceptions;
using PaymentService.Interfaces;
using System.Text.Json;

namespace PaymentService.MessageBroker
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly string _topicName;

        public KafkaConsumer(IConfiguration configuration, IServiceProvider serviceProvider, ILogger<KafkaConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _topicName = configuration["Kafka:PaymentTopic"] ?? "payment-events";

            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "kafka:29092",
                GroupId = configuration["Kafka:GroupId"] ?? "payment-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ClientId = "payment-service-consumer",
                EnableAutoCommit = true
            };

            _logger.LogInformation("Connecting to Kafka broker at {BootstrapServers}", config.BootstrapServers);

            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Kafka consumer starting...");
            _consumer.Subscribe(_topicName);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    if (consumeResult?.Message != null)
                    {
                        await HandlePaymentEvent(consumeResult.Message.Value);
                    }
                }
                catch (ConsumeException ex) when (ex.Error.Code == ErrorCode.UnknownTopicOrPart)
                {
                    _logger.LogWarning("Topic '{Topic}' not available yet. Retrying in 5 seconds...", _topicName);
                    await Task.Delay(5000, stoppingToken);
                }
                catch (KafkaException ex) when (ex.Error.IsBrokerError)
                {
                    _logger.LogWarning("Kafka broker unavailable. Retrying in 5 seconds...");
                    await Task.Delay(5000, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Kafka consumer cancellation requested.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error in Kafka consumer. Retrying in 5 seconds...");
                    await Task.Delay(5000, stoppingToken);
                }
            }

            _logger.LogInformation("Kafka consumer shutting down...");
            _consumer.Close();
        }

        private async Task HandlePaymentEvent(string messageValue)
        {
            try
            {
                var paymentEvent = ConvertJsonToDTO(messageValue);

                using var scope = _serviceProvider.CreateScope();
                var messagingService = scope.ServiceProvider.GetRequiredService<IMessagingService>();

                await messagingService.HandlePaymentEventAsync(paymentEvent);

                _logger.LogInformation("Successfully processed payment event for Order ID: {OrderId}", paymentEvent.OrderID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling payment event");
            }
        }

        private PaymentEventDTO ConvertJsonToDTO(string json)
        {
            try
            {
                var paymentEvent = JsonSerializer.Deserialize<PaymentEventDTO>(json);
                if (paymentEvent == null)
                    throw new InvalidPaymentEventDTOInputException("Deserialized PaymentEventDTO is null");

                return paymentEvent;
            }
            catch (JsonException ex)
            {
                throw new InvalidPaymentEventDTOInputException("Invalid PaymentEventDTO input", ex);
            }
        }

        public override void Dispose()
        {
            _consumer?.Dispose();
            base.Dispose();
        }
    }
}