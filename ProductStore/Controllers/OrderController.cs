using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.DTO;
using ProductStore.Interface;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository) 
        { 
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder()
        {
           if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           return Ok(await _orderRepository.GetOrders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(await _orderRepository.GetOrderById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder( [FromBody] OrderDTO orderCreateDTO)
        {
            if (orderCreateDTO == null)
            {
                return BadRequest(ModelState);
            }

            var order = _orderRepository.Add(orderCreateDTO);

            if (!order)
            {
                ModelState.AddModelError("", "Something went wrong!");
            }

            return Ok("Successfully created!");
        }
    }
}
