using ProductStore.DTO;
using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface IAddressRepository
    {
        IEnumerable<AddressDTO> GetAddresses();
        AddressDTO GetAddress(int id);
        bool AddressExist(int id);
        bool CreateAddress(AddressDTO address);
        bool UpdateAddress(AddressDTO address);
        bool DeleteAddress(AddressDTO address);
        bool Save();
    }
}
