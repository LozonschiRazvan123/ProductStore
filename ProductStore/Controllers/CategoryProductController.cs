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
using ProductStore.Repository;
using System.Data;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryProductController : ControllerBase
    {
        private readonly ICategoryProductRepository _categoryProductRepository;
        private readonly IServicePagination<CategoryProduct> _servicePagination;
        private readonly DataContext _dataContext;
        public CategoryProductController(ICategoryProductRepository categoryProductRepository, IServicePagination<CategoryProduct> servicePagination, DataContext dataContext) 
        { 
            _categoryProductRepository = categoryProductRepository;
            _servicePagination = servicePagination;
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryProducts()
        {
            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }
            return Ok(await _categoryProductRepository.GetCategoryProducts());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryProductResult(int id)
        {
            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            return Ok(await _categoryProductRepository.GetCategoryProductById(id));
        }

        [HttpGet("api/CustomerPagination")]
        public async Task<IActionResult> GetCustomerPagination([FromQuery] PaginationFilter filter)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            //IQueryable<Customer> query = _dataContext.Customers;
            var customers = await _servicePagination.GetCustomersPagination(_dataContext.CategoryProducts, filter);

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
                DataTable dataTable =  GetAddressData();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("CategoryProduct");

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
                    var table = worksheet.Tables.Add(tableRange, "CategoryProductTable");
                    table.TableStyle = TableStyles.Light1;

                    var fileBytes = package.GetAsByteArray();
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CategoryProduct.xlsx");
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
            dt.TableName = "CategoryProduct";
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("NameCategory", typeof(string));

            var categoryData = _categoryProductRepository.GetCategoryProducts().Result;
            foreach (var category in categoryData)
            {
                dt.Rows.Add(category.Id, category.NameCategory);

            }

            return dt;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryProduct([FromBody] CategoryProductDTO categoryProductDTO)
        {
            if (categoryProductDTO == null)
            {
                throw new BadRequest();
            }

            var address = _categoryProductRepository.Add(categoryProductDTO);

            if (address == false)
            {
                throw new ExistModel("Category product");
            }


            if (!address)
            {
                throw new BadRequest();
            }

            return Ok("Successfully created!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryProduct([FromBody] CategoryProductDTO categoryProductDTO, int id)
        {
            if(categoryProductDTO == null || categoryProductDTO.Id!=id)
            {
                throw new BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(!_categoryProductRepository.Update(categoryProductDTO))
            {
                throw new BadRequest();
            }

            return Ok("Successfully update!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryProduct(int id)
        {
            if(!_categoryProductRepository.ExistCategoryProduct(id))
            {
                throw new ExistModel("Category product");
            }

            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            var deleteCP = await _categoryProductRepository.GetCategoryProductById(id);

            if(!_categoryProductRepository.Delete(deleteCP))
            {
                throw new BadRequest();
            }

            return NoContent();
        }
    }
}
