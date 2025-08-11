using Microsoft.AspNetCore.Mvc;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("Get")]
        public string Get()
        {
            return "Welcome to the Order Service API!";
        }
    }
}