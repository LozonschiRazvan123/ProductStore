using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Models;
using ProductStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreTest.Repository
{
    public class AddressRepositoryTest
    {
       private async Task<DataContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new DataContext(options);
            databaseContext.Database.EnsureCreated();

            if(await databaseContext.Addresses.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Addresses.Add(
                        new Address()
                        {
                            Street = "Mihai Eminescu",
                            City = "Iasi",
                            State = "IS",
                        }
                        );
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void AddressRepository_Add_ReturnsBool()
        {
            var address = new AddressDTO()
            {
                Street = "Mihai Eminescu",
                City = "Iasi",
                State = "IS",
            };
            var dbContext = await GetDbContext();
            var addressRepository = new AddressRepository(dbContext);

            var result = addressRepository.CreateAddress(address);
            
            result.Should().BeFalse();
        }

        [Fact]
        public async Task AddressRepository_GetByIdAsync_ReturnsSuccess()
        {
            var id = 1;
            var dbContext = await GetDbContext();
            var addressRepository = new AddressRepository(dbContext);

            var result = addressRepository.GetAddress(id);
            result.Should().NotBeNull();
            result.Should().BeOfType<AddressDTO>();
        }
    }
}
