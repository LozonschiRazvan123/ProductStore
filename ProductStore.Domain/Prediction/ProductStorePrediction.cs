using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Core.Prediction
{
    public class ProductStorePrediction
    {
        [ColumnName("Score")]
        public float[] Label { get; set; }
    }
}
