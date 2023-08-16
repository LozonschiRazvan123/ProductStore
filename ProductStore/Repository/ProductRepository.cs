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
            var productDTO = _context.Products.Select(productCreateDTO => new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            }).FirstOrDefault();
            _context.Add(productDTO);
            return Save();
        }

        public bool Delete(ProductDTO product)
        {
            var productDTO = _context.Products.Where(p => p.Id == product.Id).Select(productCreate => new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            }).FirstOrDefault();
            _context.Remove(productDTO);
            return Save();
        }

        public bool ExistProduct(int id)
        {
            return _context.Products.Any(product => product.Id == id);
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            return _context.Products.Where(p => p.Id == id).Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
            }).FirstOrDefault();
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            return _context.Products.Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            }).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(ProductDTO product)
        {
            var productDTO = _context.Products.Where(p => p.Id == product.Id).Select(productCreate => new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            }).FirstOrDefault();
            _context.Update(productDTO);
            return Save();
        }
    }
}
