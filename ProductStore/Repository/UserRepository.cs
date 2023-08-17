using ProductStore.Interface;
using ProductStore.Models;

namespace ProductStore.Repository
{
    public class UserRepository : IUserRepository
    {
        public bool Add()
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public bool ExistUser()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }
    }
}
