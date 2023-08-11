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

        public bool CreateAddress(AddressDTO addressCreateDTO)
        {
            var address = _context.Addresses.Where(a => a.Street != addressCreateDTO.Street).Select(addressCreate => new Address
            {
                Id = addressCreateDTO.Id,
                City = addressCreateDTO.City,
                State = addressCreateDTO.State,
                Street = addressCreateDTO.Street
            }).FirstOrDefault();
            _context.Add(address);
            return Save();
        }

        public bool DeleteAddress(AddressDTO address)
        {
            var addressDTO = _context.Addresses.Where(a => a.Id == address.Id).Select(addressCreate => new Address
            {
                Id = address.Id,
                City = address.City,
                State = address.State,
                Street = address.Street
            }).FirstOrDefault();
            _context.Remove(addressDTO);
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

        public bool UpdateAddress(AddressDTO address)
        {
            var addressDTO = _context.Addresses.Where(a => a.Id == address.Id).Select( addressCreate => new Address
            {
                Id = address.Id,
                City = address.City,
                State = address.State,
                Street = address.Street
            }).FirstOrDefault();
            _context.Update(addressDTO);
            return Save();
        }
    }
}
