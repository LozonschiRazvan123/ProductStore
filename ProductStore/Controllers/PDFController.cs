using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductStore.Core.Interface;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFController : ControllerBase
    {
        private readonly IPDFService _pdfService;
        public PDFController(IPDFService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpGet("generateAddressesPdf")]
        public IActionResult GenerateAddressesPdf()
        {
            var pdfBytes = _pdfService.GenereateAddressPDF();

            return File(pdfBytes, "application/pdf", "addresses_info.pdf");
        }

        [HttpGet("generateCategoryProductPdf")]
        public async Task<IActionResult> GenerateCategoryProductPdf()
        {
            var pdfBytes = await _pdfService.GenereateCategoryProductPDF();

            return File(pdfBytes, "application/pdf", "categoryProduct_info.pdf");
        }

        [HttpGet("generateCustomerPdf")]
        public async Task<IActionResult> GenerateCustomerPdf()
        {
            var pdfBytes = await _pdfService.GenereateCustomerPDF();

            return File(pdfBytes, "application/pdf", "Customer_info.pdf");
        }

        [HttpGet("generateOrderPdf")]
        public async Task<IActionResult> GenerateOrderPdf()
        {
            var pdfBytes = await _pdfService.GenereateOrderPDF();

            return File(pdfBytes, "application/pdf", "Order_info.pdf");
        }

        [HttpGet("generateProductPdf")]
        public async Task<IActionResult> GenerateProductPdf()
        {
            var pdfBytes = await _pdfService.GenereateProductPDF();

            return File(pdfBytes, "application/pdf", "Product_info.pdf");
        }

        [HttpGet("generateUserPdf")]
        public async Task<IActionResult> GenerateUserPdf()
        {
            var pdfBytes = await _pdfService.GenereateUserPDF();

            return File(pdfBytes, "application/pdf", "User_info.pdf");
        }

    }
}
