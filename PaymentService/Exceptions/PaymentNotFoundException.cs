namespace PaymentService.Exceptions
{
    public class PaymentNotFoundException : Exception
    {
        public PaymentNotFoundException(string message)
            : base(message) { }

        public PaymentNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}