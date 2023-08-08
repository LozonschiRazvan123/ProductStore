using ProductStore.Models;

namespace ProductStore.Interface
{
    public interface IAddressRepository
    {
        IEnumerable<Address> GetAddresses();
        Address GetAddress(int id);
        bool AddressExist(int id);
        bool CreateAddress(Address address);
        bool UpdateAddress(Address address);
        bool DeleteAddress(Address address);
        bool Save();
    }
}
