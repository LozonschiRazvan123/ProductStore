using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id);
        bool ExistUser();
        bool Add();
        bool Update();
        bool Delete();
    }
}
