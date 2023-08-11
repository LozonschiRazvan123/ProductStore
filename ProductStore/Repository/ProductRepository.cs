using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;

namespace ProductStore.Repository
{
    public class ProductRepository: IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context) 
        {
            _context = context;
        }

        public bool Add(ProductDTO product)
        {
            var productDTO = _context.Products.Include(cp => cp.CategoryProduct).Select(product => new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryProduct = new CategoryProduct
                {
                    Id = product.CategoryProduct.Id,
                    NameCategory = product.CategoryProduct.NameCategory
                }
            }).FirstOrDefault();
            _context.Add(productDTO);
            return Save();
        }

        public bool Delete(ProductDTO product)
        {
            throw new NotImplementedException();
        }

        public bool ExistProduct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            return _context.Products.Where(p => p.Id == id).Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryProduct = new CategoryProductDTO
                {
                    Id = product.CategoryProduct.Id,
                    NameCategory = product.CategoryProduct.NameCategory
                }
            }).FirstOrDefault();
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            return _context.Products.Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryProduct = new CategoryProductDTO
                {
                    Id = product.CategoryProduct.Id,
                    NameCategory = product.CategoryProduct.NameCategory
                }
            }).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(ProductDTO product)
        {
            throw new NotImplementedException();
        }
    }
}
