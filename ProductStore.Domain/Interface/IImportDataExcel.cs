using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Core.Interface
{
    public interface IImportDataExcel
    {
        public void ImportDataFromExcel(IFormFile file);
        public Task ImportDataFromExcelCategoryProduct(IFormFile file);
        public void ImportDataFromExcelCustomer(IFormFile file);
        public void ImportDataFromExcelOrder(IFormFile file);
        public void ImportDataFromExcelProduct(IFormFile file);
        public void ImportDataFromExcelUser(IFormFile file);
    }
}
