using ProductStore.DTO;
using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO> GetUserById(Guid id);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByToken(string token);
        Task<User> GetUserByTokenVerification(string token);
        bool ExistUser(Guid id);
        bool Add(UserDTO user);
        bool Update(UserDTO user);
        bool Delete(UserDTO user);
        bool Save();
    }
}
