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
        /*private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository) 
        { 
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder()
        {
            var order = await _orderRepository.GetOrders();

            var orderDTO = order.Select(Customer => new CustomerDTO
            {
                Id = Customer.Id,
                Name = Customer.Name,
                Surname = Customer.Surname,
                Email = Customer.Email
            }).ToList();


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(customerDTO);
        }*/
    }
}
