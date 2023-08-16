using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                return BadRequest(ModelState);
            }
            return Ok(await _categoryProductRepository.GetCategoryProducts());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryProductResult(int id)
        {
            if(!ModelState.IsValid)
            { 
                return BadRequest(ModelState); 
            }

            return Ok(await _categoryProductRepository.GetCategoryProductById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryProduct([FromBody] CategoryProductDTO categoryProductDTO)
        {
            if (categoryProductDTO == null)
            {
                return BadRequest(ModelState);
            }

            var address = _categoryProductRepository.Add(categoryProductDTO);

            if (address != null)
            {
                ModelState.AddModelError("", "Category product is already exists");
            }


            if (!address)
            {
                ModelState.AddModelError("", "Something is wrong!");
            }

            return Ok("Successfully created!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryProduct([FromBody] CategoryProductDTO categoryProductDTO, int id)
        {
            if(categoryProductDTO == null || categoryProductDTO.Id!=id)
            {
                return BadRequest(ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(!_categoryProductRepository.Update(categoryProductDTO))
            {
                ModelState.AddModelError("", "Something is wrong!");
            }

            return Ok("Successfully update!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryProduct(int id)
        {
            if(!_categoryProductRepository.ExistCategoryProduct(id))
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deleteCP = await _categoryProductRepository.GetCategoryProductById(id);

            if(!_categoryProductRepository.Delete(deleteCP))
            {
                ModelState.AddModelError("", "Something is wrong!");
            }

            return NoContent();
        }
    }
}
