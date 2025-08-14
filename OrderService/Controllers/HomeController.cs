using Microsoft.AspNetCore.Mvc;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("GetHello")]
        public string Get()
        {
            return "Hello from Order Service!";
        }
    }
}