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
            var categoryProduct = _context.CategoryProducts.Where(cp => cp.NameCategory == categoryProductDTO.NameCategory).FirstOrDefault();

            if (categoryProduct == null)
            {
                var product = new CategoryProduct
                {
                    Id = categoryProductDTO.Id,
                    NameCategory = categoryProductDTO.NameCategory
                };
                _context.Add(product);
                return Save();
            }
            return false;
        }

        public bool Delete(CategoryProductDTO categoryProductDTO)
        {
            var categoryProductDTOUpdate = _context.CategoryProducts.Find(categoryProductDTO.Id);
            _context.Remove(categoryProductDTOUpdate);
            return Save();
        }

        public bool ExistCategoryProduct(int id)
        {
            return _context.CategoryProducts.Any(cp => cp.Id == id);
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
            var categoryProductUpdate = _context.CategoryProducts.Where(cp => cp.Id == categoryProductDTO.Id).FirstOrDefault();
            if (categoryProductUpdate != null)
            {
                categoryProductUpdate.Id = categoryProductDTO.Id;
                categoryProductUpdate.NameCategory = categoryProductDTO.NameCategory;
                _context.Update(categoryProductUpdate);
                return Save();
            }
            return false;
        }
    }
}
