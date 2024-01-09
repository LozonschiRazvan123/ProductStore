using Dapper;
using ProductStore.Core.Interface;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProductStore.Infrastructure.Repository
{
    public class CustomerRepositoryDapper : ICustomerRepositoryDapper
    {
        private readonly IDbConnection _context;
        public CustomerRepositoryDapper(IDbConnection context)
        {
            _context = context;
        }

        public async Task<bool> CreateCustomer(CustomerDTO customer)
        {
            var query = "INSERT INTO Customers (Name, Surname, Email) VALUES (@Name, @Surname, @Email); SELECT SCOPE_IDENTITY();";

            var parameters = new DynamicParameters();
            parameters.Add("Name", customer.Name, DbType.String);
            parameters.Add("Surname", customer.Surname, DbType.String);
            parameters.Add("Email", customer.Email, DbType.String);

            try
            {
                var newCustomerId = await _context.ExecuteScalarAsync<int>(query, parameters) > 0;
                return newCustomerId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteCustomer(int id)
        {
            var query = "DELETE FROM Customers WHERE Id = @id";
            await _context.ExecuteAsync(query, new { id });
        }

        public async Task<IEnumerable<CustomerDTO>> GetCustomers()
        {
            var query = "SELECT * FROM Customers";
            return await _context.QueryAsync<CustomerDTO>(query);
        }

        public async Task<IEnumerable<CustomerDTO>> GetCustomersId(int customerId)
        {
            var query = "SELECT * FROM Customers where id=@customerId";
            return await _context.QueryAsync<CustomerDTO>(query, new { CustomerId = customerId });
        }

        public async Task<bool> UpdateCustomer(int Id, CustomerDTO customer)
        {
            var query = "UPDATE Customers SET Name = @Name, Surname = @Surname, Email = @Email WHERE Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", Id, DbType.Int32);
            parameters.Add("Name", customer.Name, DbType.String);
            parameters.Add("Surname", customer.Surname, DbType.String);
            parameters.Add("Email", customer.Email, DbType.String);

            try
            {
                int rowsAffected = await _context.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
