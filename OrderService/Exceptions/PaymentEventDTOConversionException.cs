namespace OrderService.Exceptions
{
    public class PaymentEventDTOConversionException : Exception
    {
        public PaymentEventDTOConversionException(string message) : base(message) { }

        public PaymentEventDTOConversionException(string message, Exception innerException) : base(message, innerException) { }
    }
}