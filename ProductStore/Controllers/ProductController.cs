using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using ProductStore.ConfigurationError;
using ProductStore.Core.Interface;
using ProductStore.Data;
using ProductStore.DTO;
using ProductStore.Framework.Pagination;
using ProductStore.Interface;
using ProductStore.Models;
using System.Data;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository; 
        private readonly IServicePagination<Product> _servicePagination;
        private readonly DataContext _dataContext;
        public ProductController(IProductRepository productRepository, IServicePagination<Product> servicePagination, DataContext dataContext) 
        {
            _productRepository = productRepository;
            _servicePagination = servicePagination;
            _dataContext = dataContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            return Ok(await _productRepository.GetProducts());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            return Ok(await _productRepository.GetProductById(id));
        }

        [HttpGet("api/CustomerPagination")]
        public async Task<IActionResult> GetCustomerPagination([FromQuery] PaginationFilter filter)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            //IQueryable<Customer> query = _dataContext.Customers;
            var customers = await _servicePagination.GetCustomersPagination(_dataContext.Products, filter);

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
                DataTable dataTable = GetAddressData();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Product");

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
                    var table = worksheet.Tables.Add(tableRange, "ProductTable");
                    table.TableStyle = TableStyles.Light1;

                    var fileBytes = package.GetAsByteArray();
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Product.xlsx");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in Excel {ex.Message}");
            }
        }

        private DataTable GetAddressData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Product";
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Price", typeof(int));

            var categoryData = _productRepository.GetProducts().Result;
            foreach (var customer in categoryData)
            {
                dt.Rows.Add(customer.Id, customer.Name, customer.Price);

            }

            return dt;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO product)
        {
            if (product == null)
            {
                throw new BadRequest();
            }

            var productCreate = _productRepository.Add(product);

            if (productCreate == false)
            {
                throw new ExistModel("Product");
            }


            if (!productCreate)
            {
                throw new BadRequest();
            }

            return Ok("Successfully created!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if(!_productRepository.ExistProduct(id))
            {
                throw new AppException("Product", id.ToString());
            }
            var productDelete = await _productRepository.GetProductById(id);
            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            if (!_productRepository.Delete(productDelete))
            {
                throw new BadRequest();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO productDTOUpdate, int id)
        {
            if(productDTOUpdate == null || productDTOUpdate.Id!=id)
            {
                throw new BadRequest();
            }

            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }
            if(!_productRepository.Update(productDTOUpdate))
            {
                throw new BadRequest();
            }
            return Ok("Update successfully!");
        }
    }
}
