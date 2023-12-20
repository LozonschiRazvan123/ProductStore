using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using ProductStore.Core.Prediction;
using ProductStore.DTO;

namespace ProductStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStoreController : ControllerBase
    {
        private readonly MLContext _mlContext;
        private readonly PredictionEngine<ProductStoreData, ProductStorePrediction> _predictionEngine;

        public ProductStoreController()
        {
            _mlContext = new MLContext();
            var modelPath = Path.Combine(Directory.GetCurrentDirectory(), "ProductStoreController.zip");
            var model = _mlContext.Model.Load(modelPath, out _);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ProductStoreData, ProductStorePrediction>(model);
        }

        [HttpPost("predict")]
        public ActionResult<float[]> PredictProductLabel([FromBody] ProductStoreData productData)
        {
            var prediction = _predictionEngine.Predict(productData);
            return prediction.Label;
        }
    }
}
