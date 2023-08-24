using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.ConfigurationError;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Repository;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryProductController : ControllerBase
    {
        private readonly ICategoryProductRepository _categoryProductRepository;
        public CategoryProductController(ICategoryProductRepository categoryProductRepository) 
        { 
            _categoryProductRepository = categoryProductRepository;
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
