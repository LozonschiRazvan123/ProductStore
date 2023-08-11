using ProductStore.DTO;
using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<ProductDTO> GetProductById(int id);
        bool ExistProduct(int id);
        bool Add(ProductDTO product);
        bool Update(ProductDTO product);
        bool Delete(ProductDTO product);
        bool Save();
    }
}
