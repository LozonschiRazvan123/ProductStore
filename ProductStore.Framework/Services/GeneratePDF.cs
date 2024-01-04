using ProductStore.Core.Interface;
using ProductStore.DTO;
using ProductStore.Interface;
using ProductStore.Models;
using ProductStore.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Framework.Services
{
    public class GeneratePDF : IPDFService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryProductRepository _categoryProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        public GeneratePDF(IAddressRepository addressRepository, IOrderRepository orderRepository, ICategoryProductRepository categoryProductRepository, IProductRepository productRepository, IUserRepository userRepository, ICustomerRepository customerRepository)
        {
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _categoryProductRepository = categoryProductRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
        }
        public byte[] GenereateAddressPDF()
        {
            var addressCount = _addressRepository.GetAddresses();
            var htmlContent = "<html><body>";

            foreach (var addressData in addressCount)
            {
                htmlContent += $@"
                <h1>Address Information</h1>
                <p>ID: {addressData.Id}</p>
                <p>Street: {addressData.Street}</p>
                <p>City: {addressData.City}</p>
                <p>State: {addressData.State}</p>
                <hr />";
            }

            htmlContent += "</body></html>";

            var htmlToPdf = new HtmlToPdf();
            var pdfDocument = htmlToPdf.RenderHtmlAsPdf(htmlContent);

            return pdfDocument.BinaryData;
        }

        public async Task<byte[]> GenereateCategoryProductPDF()
        {
            var categoryProductCount = await _categoryProductRepository.GetCategoryProducts();
            var htmlContent = "<html><body>";
            foreach ( var categoryProduct in categoryProductCount )
            {
                htmlContent+= $@"<h1>Category Product Information</h1>
                    <p>ID:{categoryProduct.Id}</p>
                    <p>Name Category:{categoryProduct.NameCategory} </p>
                    <hr />";
            }
            htmlContent += "</body></html>";
            var htmlToPdf = new HtmlToPdf();
            var pdfDocument = htmlToPdf.RenderHtmlAsPdf(htmlContent);

            return pdfDocument.BinaryData;
        }

        public async Task<byte[]> GenereateCustomerPDF()
        {
            var customers = await _customerRepository.GetCustomers();
            var htmlContent = "<html><body>";
            foreach ( var customer in customers)
            {
                htmlContent += $@"<h1>Customer Information</h1>
                 <p>ID:{customer.Id}</p>
                 <p>Name:{customer.Name}</p>
                 <p>Surname:{customer.Surname}</p>
                 <p>Email:{customer.Email}</p>
                 <hr/>";
            }
            htmlContent += "</body></html>";
            var htmlToPdf = new HtmlToPdf();
            var pdfDocument = htmlToPdf.RenderHtmlAsPdf(htmlContent);

            return pdfDocument.BinaryData;
        }

        public async Task<byte[]> GenereateOrderPDF()
        {
            var orders = await _orderRepository.GetOrders();
            var htmlContent = "<html><body>";
            foreach (var order in orders)
            {
                htmlContent += $@"<h1>Order Information</h1>
                 <p>ID:{order.Id}</p>
                 <p>Date:{order.DateTime}</p>
                 <hr/>";
            }
            htmlContent += "</body></html>";
            var htmlToPdf = new HtmlToPdf();
            var pdfDocument = htmlToPdf.RenderHtmlAsPdf(htmlContent);

            return pdfDocument.BinaryData;
        }

        public async Task<byte[]> GenereateProductPDF()
        {
            var products = await _productRepository.GetProducts();
            var htmlContent = "<html><body>";
            foreach (var product in products)
            {
                htmlContent += $@"<h1>Order Information</h1>
                 <p>ID:{product.Id}</p>
                 <p>Name:{product.Name}</p>
                 <p>Price:{product.Price}</p>
                 <hr/>";
            }
            htmlContent += "</body></html>";
            var htmlToPdf = new HtmlToPdf();
            var pdfDocument = htmlToPdf.RenderHtmlAsPdf(htmlContent);

            return pdfDocument.BinaryData;
        }

        public async Task<byte[]> GenereateUserPDF()
        {
            var users = await _userRepository.GetUsers();
            var htmlContent = "<html><body>";
            foreach (var user in users)
            {
                htmlContent += $@"<h1>Order Information</h1>
                 <p>ID:{user.Id}</p>
                 <p>Name:{user.UserName}</p>
                 <hr/>";
            }
            htmlContent += "</body></html>";
            var htmlToPdf = new HtmlToPdf();
            var pdfDocument = htmlToPdf.RenderHtmlAsPdf(htmlContent);

            return pdfDocument.BinaryData;
        }
    }
}
