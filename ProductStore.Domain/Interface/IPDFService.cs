using ProductStore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Core.Interface
{
    public interface IPDFService
    {
        byte[] GenereateAddressPDF();
        Task<byte[]> GenereateCategoryProductPDF();
        Task<byte[]> GenereateCustomerPDF();
        Task<byte[]> GenereateOrderPDF();
        Task<byte[]> GenereateProductPDF();
        Task<byte[]> GenereateUserPDF();
    }
}
