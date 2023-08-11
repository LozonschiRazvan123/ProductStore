using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;

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

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderDTO orderUpdateDTO)
        {
            if (orderUpdateDTO == null || orderId != orderUpdateDTO.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!_orderRepository.Update(orderUpdateDTO))
            {
                ModelState.AddModelError("", "Something is wrong!");
            }

            return Ok("Successfully update!");
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            if (!_orderRepository.ExistOrder(orderId))
            {
                return NotFound();
            }

            var customerDelete = await _orderRepository.GetOrderById(orderId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_orderRepository.Delete(customerDelete))
            {
                ModelState.AddModelError("", "Something is wrong!");
            }

            return NoContent();
        }
    }
}
