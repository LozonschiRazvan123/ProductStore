using ProductStore.DTO;

namespace ProductStore.Interface
{
    public interface IImageRepository
    {
        bool UpdateImage(UserDTO user);
        bool DeleteImage(UserDTO user);
        bool AddImage(UserDTO user);
        bool Save();
    }
}
