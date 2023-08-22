using ProductStore.DTO;
using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO> GetUserById(int id);
        bool ExistUser(int id);
        bool Add(User user);
        bool Update(UserDTO user);
        bool Delete(UserDTO user);
        bool Save();
    }
}
