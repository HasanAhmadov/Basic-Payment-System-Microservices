namespace OrderService.Exceptions
{
    public class InvalidPaymentEventDTOInputException : Exception
    {
        public InvalidPaymentEventDTOInputException(string message) : base(message) { }

        public InvalidPaymentEventDTOInputException(string message, Exception innerException) : base(message, innerException) { }
    }
}