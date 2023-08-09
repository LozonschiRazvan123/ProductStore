using ProductStore.Data;
using ProductStore.DTO;
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

        public bool DeleteAddress(AddressDTO address)
        {
            _context.Remove(address);
            return Save();
        }

        public AddressDTO GetAddress(int id)
        {
            //return _context.Addresses.Where(a => a.Id == id).FirstOrDefault();

            return _context.Addresses.Where(a => a.Id == id).Select(w => new AddressDTO()
            {
                Id = w.Id,
                City = w.City,
                State = w.State,
                Street = w.Street
            }).FirstOrDefault();
        }

        public IEnumerable<AddressDTO> GetAddresses()
        {
            return _context.Addresses.Select(address => new AddressDTO
            {
                Id = address.Id,
                Street = address.Street,
                City = address.City,
                State = address.State
            }).ToList();
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
