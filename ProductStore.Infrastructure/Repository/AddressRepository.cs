using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;
using System;

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
            var existingAddress = _context.Addresses.Where(a => a.Street == addressCreateDTO.Street).FirstOrDefault();

            if (existingAddress == null)
            {
                var address = new Address
                {
                    City = addressCreateDTO.City,
                    State = addressCreateDTO.State,
                    Street = addressCreateDTO.Street
                };

                _context.Add(address);
                return Save();
            }
            return false;
        }

    public bool DeleteAddress(AddressDTO address)
        {
            var existingAddress = _context.Addresses.Find(address.Id);
            _context.Remove(existingAddress);
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
            var existingAddress = _context.Addresses.FirstOrDefault(a => a.Id == address.Id);

            if (existingAddress != null)
            {
                existingAddress.Id = address.Id;
                existingAddress.City = address.City;
                existingAddress.State = address.State;
                existingAddress.Street = address.Street;

                _context.Update(existingAddress);
                return Save();
            }
            return false;
        }
    }
}
