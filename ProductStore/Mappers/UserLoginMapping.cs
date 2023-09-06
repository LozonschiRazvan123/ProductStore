using ProductStore.DTO;
using ProductStore.Models;

namespace ProductStore.Mappers
{
    public static class UserLoginMapping
    {
        public static UserDTO MapUserRole(User user)
        {
            return new UserDTO { Role = user.Role };
        }
    }
}
