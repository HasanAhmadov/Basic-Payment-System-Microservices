using OrderService.Models.Enums;

namespace OrderService.Interfaces
{
    public interface IMessagingDataService
    {
        Task UpdateOrderStatusAsync(long orderID, PaymentStatus paymentStatus);
    }
}