using ProductStore.Data;
using ProductStore.Interface;
using ProductStore.Models;

namespace ProductStore.Repository
{
    public class AddressRepository: IAddressRepository
    {
        private readonly DataContext _context;
        public AddressRepository(DataContext context) 
        { 
            _context = context;
        }

        public bool AddressExist(int id)
        {
            return _context.Addresses.Any(a => a.Id == id);    
        }

        public bool CreateAddress(Address address)
        {
            _context.Add(address);
            return Save();
        }

        public bool DeleteAddress(Address address)
        {
            _context.Remove(address);
            return Save();
        }

        public Address GetAddress(int id)
        {
            return _context.Addresses.Where(a => a.Id == id).FirstOrDefault();
        }

        public IEnumerable<Address> GetAddresses()
        {
            return _context.Addresses.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateAddress(Address address)
        {
            _context.Update(address);
            return Save();
        }
    }
}
