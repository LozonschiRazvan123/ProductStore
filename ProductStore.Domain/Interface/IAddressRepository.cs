

using ProductStore.DTO;

namespace ProductStore.Interface
{
    public interface IAddressRepository
    {
        IEnumerable<AddressDTO> GetAddresses();
        Task AddBulkAddressesAsync();
        Task AddWithoutBulkAddressesAsync();
        Task DeleteAllBulkAddressesAsync();
        Task DeleteWithoutAllBulkAddressesAsync();

        Task DeleteAIdsBulkAddressesAsync(List<int> addressId);
        Task DeleteAIdsWithoutBulkAddressesAsync(List<int> addressId);
        Task UpdateBulkAddress();
        Task UpdateWithoutBulkAddress();
        AddressDTO GetAddress(int id);
        bool AddressExist(int id);
        bool CreateAddress(AddressDTO address);
        bool UpdateAddress(AddressDTO address);
        bool DeleteAddress(AddressDTO address);
        bool Save();
    }
}
