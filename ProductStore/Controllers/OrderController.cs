using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.ConfigurationError;
using ProductStore.Core.Interface;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Framework.Pagination;
using ProductStore.Interface;
using ProductStore.Models;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IServicePagination<Order> _servicePagination;
        private readonly DataContext _dataContext;
        public OrderController(IOrderRepository orderRepository, IServicePagination<Order> servicePagination, DataContext dataContext) 
        { 
            _orderRepository = orderRepository;
            _servicePagination = servicePagination;
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder()
        {
           if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }
           return Ok(await _orderRepository.GetOrders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }
            return Ok(await _orderRepository.GetOrderById(id));
        }

        [HttpGet("api/CustomerPagination")]
        public async Task<IActionResult> GetCustomerPagination([FromQuery] PaginationFilter filter)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            //IQueryable<Customer> query = _dataContext.Customers;
            var customers = await _servicePagination.GetCustomersPagination(_dataContext.Orders, filter);

            var response = new
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = customers.TotalPages,
                TotalRecords = customers.TotalRecords,
                Customers = customers.Results
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder( [FromBody] OrderDTO orderCreateDTO)
        {
            if (orderCreateDTO == null)
            {
                throw new BadRequest();
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
                throw new BadRequest();
            }

            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }
            if (!_orderRepository.Update(orderUpdateDTO))
            {
                throw new BadRequest();
            }

            return Ok("Successfully update!");
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            if (!_orderRepository.ExistOrder(orderId))
            {
                throw new ExistModel("Order");
            }

            var customerDelete = await _orderRepository.GetOrderById(orderId);
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            if (!_orderRepository.Delete(customerDelete))
            {
                throw new BadRequest();
            }

            return NoContent();
        }
    }
}
