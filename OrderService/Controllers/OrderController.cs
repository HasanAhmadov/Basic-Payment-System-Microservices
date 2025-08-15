using Microsoft.AspNetCore.Mvc;
using OrderService.Exceptions;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderDataService _orderDataService;

        public OrderController(IOrderDataService orderDataService)
        {
            _orderDataService = orderDataService;
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            try
            {
                var createdOrder = await _orderDataService.CreateOrderAsync(order);
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderID }, createdOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            var orders = await _orderDataService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("GetOrderById/{id}")]
        public async Task<ActionResult<Order>> GetOrderById(long id)
        {
            try
            {
                var order = await _orderDataService.GetOrderByIdAsync(id);
                return Ok(order);
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("UpdateOrderById/{id}")]
        public async Task<ActionResult<Order>> UpdateOrder(long id, [FromBody] Order order)
        {
            try
            {
                var updatedOrder = await _orderDataService.UpdateOrderByIdAsync(id, order);
                return Ok(updatedOrder);
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteOrderById/{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            try
            {
                await _orderDataService.DeleteOrderByIdAsync(id);
                return NoContent();
            }
            catch (OrderNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}