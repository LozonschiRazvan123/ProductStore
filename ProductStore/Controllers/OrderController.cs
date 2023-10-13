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
using ProductStore.Framework.Services;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IServicePagination<Order> _servicePagination;
        private readonly DataContext _dataContext;
        private readonly IGetDataExcel _excel;
        private readonly IImportDataExcel _importDataExcel;
        public OrderController(IOrderRepository orderRepository, IServicePagination<Order> servicePagination, DataContext dataContext, IImportDataExcel importDataExcel) 
        { 
            _orderRepository = orderRepository;
            _servicePagination = servicePagination;
            _dataContext = dataContext;
            _importDataExcel = importDataExcel;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder()
        {
           if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }
           return Ok(await _orderRepository.GetOrders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if(!ModelState.IsValid)
            {
                throw new BadRequest();
            }
            return Ok(await _orderRepository.GetOrderById(id));
        }

        [HttpGet("api/CustomerPagination")]
        public async Task<IActionResult> GetCustomerPagination([FromQuery] PaginationFilter filter)
        {
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            //IQueryable<Customer> query = _dataContext.Customers;
            var customers = await _servicePagination.GetCustomersPagination(_dataContext.Orders, filter);

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
                DataTable dataTable = _excel.GetOrderData();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Order");

                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                    }

                    for (int row = 0; row < dataTable.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataTable.Columns.Count; col++)
                        {
                            var cell = worksheet.Cells[row + 2, col + 1];
                            cell.Value = dataTable.Rows[row][col];

                            if (dataTable.Columns[col].DataType == typeof(DateTime))
                            {
                                cell.Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                            }
                        }
                    }

                    var tableRange = worksheet.Cells[1, 1, dataTable.Rows.Count + 1, dataTable.Columns.Count];
                    var table = worksheet.Tables.Add(tableRange, "OrderTable");
                    table.TableStyle = TableStyles.Light1;

                    var fileBytes = package.GetAsByteArray();
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Order.xlsx");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in Excel {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder( [FromBody] OrderDTO orderCreateDTO)
        {
            if (orderCreateDTO == null)
            {
                throw new BadRequest();
            }

            var order = _orderRepository.Add(orderCreateDTO);

            if (!order)
            {
                ModelState.AddModelError("", "Something went wrong!");
            }

            return Ok("Successfully created!");
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
                        _importDataExcel.ImportDataFromExcelOrder(file);
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

        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] OrderDTO orderUpdateDTO)
        {
            if (orderUpdateDTO == null || orderId != orderUpdateDTO.Id)
            {
                throw new BadRequest();
            }

            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }
            if (!_orderRepository.Update(orderUpdateDTO))
            {
                throw new BadRequest();
            }

            return Ok("Successfully update!");
        }

        [HttpPut("ImportExcel")]
        public IActionResult ImportExcelOrder(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        _importDataExcel.ImportDataExcelUpdateOrder(file);
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

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            if (!_orderRepository.ExistOrder(orderId))
            {
                throw new ExistModel("Order");
            }

            var customerDelete = await _orderRepository.GetOrderById(orderId);
            if (!ModelState.IsValid)
            {
                throw new BadRequest();
            }

            if (!_orderRepository.Delete(customerDelete))
            {
                throw new BadRequest();
            }

            return NoContent();
        }
    }
}
