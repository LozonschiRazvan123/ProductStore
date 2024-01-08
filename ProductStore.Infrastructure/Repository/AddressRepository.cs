using EFCore.BulkExtensions;
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

        public async Task AddBulkAddressesAsync()
        {
            List<Address> addresses = new List<Address>();

            for (int i = 50; i < 100000; i++)
            {
                addresses.Add(new Address()
                {
                    City = "City_" + i,
                    State = "State_" + i,
                    Street = "Street_" + i
                });
            }

           await _context.BulkInsertAsync(addresses);
        }

        public bool AddressExist(int id)
        {
            return _context.Addresses.Any(a => a.Id == id);    
        }

        public async Task AddWithoutBulkAddressesAsync()
        {
            List<Address> addresses = new List<Address>();

            for (int i = 10000; i < 100000; i++)
            {
                addresses.Add(new Address()
                {
                    City = "City_" + i,
                    State = "State_" + i,
                    Street = "Street_" + i
                });
            }

            _context.Addresses.AddRange(addresses);
            await _context.SaveChangesAsync();
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

        public async Task DeleteAIdsBulkAddressesAsync(List<int> addressId)
        {
            List<Address> addressesToDelete =  _context.Addresses
                .Where(a => addressId.Contains(a.Id))
                .ToList();

            if (addressesToDelete.Any())
            {
                await _context.BulkDeleteAsync(addressesToDelete);
            }
        }

        public async Task DeleteAIdsWithoutBulkAddressesAsync(List<int> addressId)
        {
            List<Address> addressesToDelete = _context.Addresses
                .Where(a => addressId.Contains(a.Id))
                .ToList();

            if (addressesToDelete.Any())
            {
                _context.RemoveRange(addressesToDelete);
            }
        }

        public async Task DeleteAllBulkAddressesAsync()
        {
            List<Address> employees = new(); 
           
            var addresses = _context.Addresses.ToList();
            await _context.BulkDeleteAsync(addresses);
        }

        public async Task DeleteWithoutAllBulkAddressesAsync()
        {
            List<Address> employees = new();

            var addresses = _context.Addresses.ToList();
            _context.RemoveRange(addresses);
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

        public async Task UpdateBulkAddress()
        {
            List<Address> addresses = new();
            for (int i = 50; i < 100000; i++)
            {
                addresses.Add(new Address()
                {
                    Id = (i + 1),
                    City = "CityUpdate_" + i,
                    State = "StateUpdate_" + i,
                    Street = "StreetUpdate_" + i
                });
            }
            await _context.BulkUpdateAsync(addresses);
        }

        public async Task UpdateWithoutBulkAddress()
        {
            List<Address> addresses = new();
            for (int i = 100000; i < 100100; i++)
            {
                addresses.Add(new Address()
                {
                    Id = (i + 1),
                    City = "CityUpdate_" + i,
                    State = "StateUpdate_" + i,
                    Street = "StreetUpdate_" + i
                });
            }
            _context.UpdateRange(addresses);
        }
    }
}
