using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OrderService.Exceptions
{
    public class HttpResponseExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is OrderNotFoundException)
            {
                context.Result = new NotFoundObjectResult(new { Error = context.Exception.Message });
                context.ExceptionHandled = true;
            }
        }
    }
}