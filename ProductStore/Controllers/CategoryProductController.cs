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
    public class CategoryProductController : ControllerBase
    {
        private readonly ICategoryProductRepository _categoryProductRepository;
        private readonly IServicePagination<CategoryProduct> _servicePagination;
        private readonly DataContext _dataContext;
        public CategoryProductController(ICategoryProductRepository categoryProductRepository, IServicePagination<CategoryProduct> servicePagination, DataContext dataContext) 
        { 
            _categoryProductRepository = categoryProductRepository;
            _servicePagination = servicePagination;
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryProducts()
        {
            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }
            return Ok(await _categoryProductRepository.GetCategoryProducts());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryProductResult(int id)
        {
            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            return Ok(await _categoryProductRepository.GetCategoryProductById(id));
        }

        [HttpGet("api/CustomerPagination")]
        public async Task<IActionResult> GetCustomerPagination([FromQuery] PaginationFilter filter)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            //IQueryable<Customer> query = _dataContext.Customers;
            var customers = await _servicePagination.GetCustomersPagination(_dataContext.CategoryProducts, filter);

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
        public async Task<IActionResult> CreateCategoryProduct([FromBody] CategoryProductDTO categoryProductDTO)
        {
            if (categoryProductDTO == null)
            {
                throw new BadRequest();
            }

            var address = _categoryProductRepository.Add(categoryProductDTO);

            if (address == false)
            {
                throw new ExistModel("Category product");
            }


            if (!address)
            {
                throw new BadRequest();
            }

            return Ok("Successfully created!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryProduct([FromBody] CategoryProductDTO categoryProductDTO, int id)
        {
            if(categoryProductDTO == null || categoryProductDTO.Id!=id)
            {
                throw new BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(!_categoryProductRepository.Update(categoryProductDTO))
            {
                throw new BadRequest();
            }

            return Ok("Successfully update!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryProduct(int id)
        {
            if(!_categoryProductRepository.ExistCategoryProduct(id))
            {
                throw new ExistModel("Category product");
            }

            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            var deleteCP = await _categoryProductRepository.GetCategoryProductById(id);

            if(!_categoryProductRepository.Delete(deleteCP))
            {
                throw new BadRequest();
            }

            return NoContent();
        }
    }
}
