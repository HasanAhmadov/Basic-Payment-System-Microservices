using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("GetHello")]
        public string Get()
        {
            return "Hello from Payment Service!";
        }
    }
}