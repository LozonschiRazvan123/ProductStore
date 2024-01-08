

using ProductStore.DTO;

namespace ProductStore.Interface
{
    public interface IAddressRepository
    {
        IEnumerable<AddressDTO> GetAddresses();
        Task AddBulkAddressesAsync();
        Task DeleteAllBulkAddressesAsync();
        Task DeleteAIdsBulkAddressesAsync(List<int> addressId);
        Task UpdateBulkAddress();
        AddressDTO GetAddress(int id);
        bool AddressExist(int id);
        bool CreateAddress(AddressDTO address);
        bool UpdateAddress(AddressDTO address);
        bool DeleteAddress(AddressDTO address);
        bool Save();
    }
}
