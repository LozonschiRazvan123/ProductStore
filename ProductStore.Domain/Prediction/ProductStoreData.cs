using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Core.Prediction
{
    public class ProductStoreData
    {
        [LoadColumn(0)] // ProductID
        public float ProductID { get; set; }

        [LoadColumn(1)] // ProductName
        public string ProductName { get; set; }

        [LoadColumn(2)] // Price
        public float Price { get; set; }

        [LoadColumn(3)] // Category
        public string Category { get; set; }

        [LoadColumn(4)] // IsPopular
        public float IsPopular { get; set; }

        [LoadColumn(5)] // Rating
        public float Rating { get; set; }

        [LoadColumn(6)] // Reviews
        public float Reviews { get; set; }
    }
}
