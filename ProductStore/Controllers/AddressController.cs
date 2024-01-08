using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using ProductStore.ConfigurationError;
using ProductStore.Core.Interface;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Framework.Pagination;
using ProductStore.Framework.Services;
using ProductStore.Interface;
using ProductStore.Models;
using ProductStore.Repository;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IServicePagination<Address> _servicePagination;
        private readonly DataContext _dataContext;
        private readonly IGetDataExcel _excel;
        private readonly IImportDataExcel _importDataExcel;
        public AddressController(IAddressRepository addressRepository, IServicePagination<Address> servicePagination, DataContext dataContext, IGetDataExcel excel, IImportDataExcel importDataExcel)
        {
            _addressRepository = addressRepository;
            _servicePagination = servicePagination;
            _dataContext = dataContext;
            _excel = excel;
            _importDataExcel = importDataExcel;
        }

        [HttpGet]
        public IActionResult GetAddress() 
        {
            return Ok(_addressRepository.GetAddresses());
        }

        [HttpGet("{id}")]
        public IActionResult GetAddressById(int id)
        {
            if(_addressRepository.GetAddress(id) == null)
            {
                throw new AppException("Address", id.ToString());
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_addressRepository.GetAddress(id));
        }

        [HttpGet("api/CustomerPagination")]
        public async Task<IActionResult> GetCustomerPagination([FromQuery] PaginationFilter filter)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            //IQueryable<Customer> query = _dataContext.Customers;
            var customers = await _servicePagination.GetCustomersPagination(_dataContext.Addresses, filter);

            var response = new
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = customers.TotalPages,
                TotalRecords = customers.TotalRecords,
                Customers = customers.Results
            };

            return Ok(response);
        }

        [HttpGet("ExportExcel")]
        public IActionResult ExportExcel()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                DataTable dataTable = _excel.GetAddressData();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Adress");

                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                    }

                    for (int row = 0; row < dataTable.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataTable.Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                        }
                    }

                    var tableRange = worksheet.Cells[1, 1, dataTable.Rows.Count + 1, dataTable.Columns.Count];
                    var table = worksheet.Tables.Add(tableRange, "AdreseTable");
                    table.TableStyle = TableStyles.Light1;

                    var fileBytes = package.GetAsByteArray();
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Adress.xlsx");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in Excel: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateAddress([FromBody] AddressDTO addressCreateDTO)
        {
            if(addressCreateDTO == null)
            {
                return BadRequest(ModelState);
            }

            var address = _addressRepository.CreateAddress(addressCreateDTO);

            if(address == false)
            {
                throw new ExistModel("Address");
            }


            if(!address)
            {
                throw new BadRequest();
            }

            return Ok("Successfully created!");
        }

        [HttpPost("AddBulkDataTable")]
        public async Task<IActionResult> AddBulkData()
        {
            await _addressRepository.AddBulkAddressesAsync();
            return Ok("Successfully added");
        }

        [HttpPost("ImportExcel")]
        public IActionResult ImportExcel(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        _importDataExcel.ImportDataFromExcel(file);
                    }

                    return Ok("Awsome");
                }
                else
                {
                    return BadRequest("Naspa");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("{addressId}")]
        public IActionResult UpdateAddress(int addressId, [FromBody] AddressDTO updateAddress)
        {
            if (updateAddress == null || addressId != updateAddress.Id)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(!_addressRepository.UpdateAddress(updateAddress))
            {
                throw new BadRequest();
            }

            return Ok("Successfully update!");
        }

        [HttpPut("ImportExcel")]
        public IActionResult ImportExcelUpdate(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        _importDataExcel.ImportDataExcelUpdateAddress(file);
                    }

                    return Ok("Awsome");
                }
                else
                {
                    return BadRequest("Naspa");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("UpdateBulkAddress")]
        public async Task<IActionResult> UpdateBulkAddress()
        {
            await _addressRepository.UpdateBulkAddress();
            return Ok("Update successfully");
        }

        [HttpDelete("{addressId}")]
        public IActionResult DeleteAddress(int addressId)
        {
            if(!_addressRepository.AddressExist(addressId))
            {
                throw new AppException("Address", addressId.ToString());
            }

            var addressDelete = _addressRepository.GetAddress(addressId);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!_addressRepository.DeleteAddress(addressDelete))
            {
                throw new BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("DeleteBulkData")]
        public async Task<IActionResult> DeleteBulkData()
        {
            await _addressRepository.DeleteAllBulkAddressesAsync();
            //await _addressRepository.DeleteAIdsBulkAddressesAsync(addressId);
            return Ok("Successfully delete");
        }
    }
}
