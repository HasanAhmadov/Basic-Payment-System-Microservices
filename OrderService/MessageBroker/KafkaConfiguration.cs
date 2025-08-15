namespace OrderService.MessageBroker
{
    public class KafkaConfiguration
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public string PaymentTopic { get; set; } = string.Empty;
        public string OrderTopic { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
    }
}