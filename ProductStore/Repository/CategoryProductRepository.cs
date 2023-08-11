using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;

namespace ProductStore.Repository
{
    public class CategoryProductRepository: ICategoryProductRepository
    {
        private readonly DataContext _context;
        public CategoryProductRepository(DataContext context) 
        {
            _context = context;
        }

        public bool Add(CategoryProductDTO categoryProductDTO)
        {
            var categoryProduct =  _context.CategoryProducts.Where(cp => cp.NameCategory != categoryProductDTO.NameCategory).Select(product => new CategoryProduct
            {
                Id = categoryProductDTO.Id,
                NameCategory = categoryProductDTO.NameCategory
            }).FirstOrDefault();

            _context.Add(categoryProduct);
            return Save();
        }

        public bool Delete(CategoryProductDTO categoryProductDTO)
        {
            throw new NotImplementedException();
        }

        public bool ExistCategoryProduct()
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryProductDTO> GetCategoryProductById(int id)
        {
            return _context.CategoryProducts.Where(cp => cp.Id == id).Select(product => new CategoryProductDTO
            {
                Id = product.Id,
                NameCategory = product.NameCategory
            }).FirstOrDefault();
        }

        public async Task<IEnumerable<CategoryProductDTO>> GetCategoryProducts()
        {
            return _context.CategoryProducts.Select(product => new CategoryProductDTO
            {
                Id = product.Id,
                NameCategory = product.NameCategory
            }).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(CategoryProductDTO categoryProductDTO)
        {
            throw new NotImplementedException();
        }
    }
}
