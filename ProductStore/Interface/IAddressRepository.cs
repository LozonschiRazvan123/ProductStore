using ProductStore.DTO;
using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface IAddressRepository
    {
        IEnumerable<AddressDTO> GetAddresses();
        AddressDTO GetAddress(int id);
        bool AddressExist(int id);
        bool CreateAddress(Address address);
        bool UpdateAddress(Address address);
        bool DeleteAddress(AddressDTO address);
        bool Save();
    }
}
