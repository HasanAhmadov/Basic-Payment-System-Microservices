using Microsoft.AspNetCore.Mvc;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("Get")]
        public string Get()
        {
            return "Welcome to the Payment Service API!";
        }
    }
}