using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Repository;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository; 
        public ProductController(IProductRepository productRepository) 
        {
            _productRepository = productRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _productRepository.GetProducts());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _productRepository.GetProductById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO product)
        {
            if (product == null)
            {
                return BadRequest(ModelState);
            }

            var productCreate = _productRepository.Add(product);

            if (productCreate != null)
            {
                ModelState.AddModelError("", "Product is already exists");
            }


            if (!productCreate)
            {
                ModelState.AddModelError("", "Something is wrong!");
            }

            return Ok("Successfully created!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if(!_productRepository.ExistProduct(id))
            {
                return NotFound();
            }
            var productDelete = await _productRepository.GetProductById(id);
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_productRepository.Delete(productDelete))
            {
                ModelState.AddModelError("", "Something is wrong!");
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO productDTOUpdate, int id)
        {
            if(productDTOUpdate == null || productDTOUpdate.Id!=id)
            {
                return BadRequest(ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            if(!_productRepository.Update(productDTOUpdate))
            {
                ModelState.AddModelError("", "Something is wrong!");
            }
            return Ok("Update successfully!");
        }
    }
}
