using ProductStore.DTO;

namespace ProductStore.Interface
{
    public interface ICategoryProductRepository
    {
        Task<IEnumerable<CategoryProductDTO>> GetCategoryProducts();
        Task<CategoryProductDTO> GetCategoryProductById(int id);
        bool Add(CategoryProductDTO categoryProductDTO);
        bool Delete(CategoryProductDTO categoryProductDTO);
        bool Update(CategoryProductDTO categoryProductDTO);
        bool Save();
        bool ExistCategoryProduct(int id);
    }
}
