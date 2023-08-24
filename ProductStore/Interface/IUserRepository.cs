using ProductStore.DTO;
using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByToken(string token);
        Task<User> GetUserByTokenVerification(string token);
        bool ExistUser(int id);
        bool Add(User user);
        bool Update(UserDTO user);
        bool Delete(UserDTO user);
        bool Save();
    }
}
