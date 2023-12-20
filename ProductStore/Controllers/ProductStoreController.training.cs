﻿﻿// This file was auto-generated by ML.NET Model Builder. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Microsoft.ML;

namespace ProductStore
{
    public partial class ProductStoreController
    {
        /// <summary>
        /// Retrains model using the pipeline generated as part of the training process. For more information on how to load data, see aka.ms/loaddata.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="trainData"></param>
        /// <returns></returns>
        public static ITransformer RetrainPipeline(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            var model = pipeline.Fit(trainData);

            return model;
        }
        /// <summary>
        /// build the pipeline that is used from model builder. Use this function to retrain model.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations
            var pipeline = mlContext.Transforms.Categorical.OneHotEncoding(@"Category", @"Category", outputKind: OneHotEncodingEstimator.OutputKind.Indicator)      
                                    .Append(mlContext.Transforms.ReplaceMissingValues(new []{new InputOutputColumnPair(@"ProductID", @"ProductID"),new InputOutputColumnPair(@"Price", @"Price"),new InputOutputColumnPair(@"Rating", @"Rating"),new InputOutputColumnPair(@"Reviews", @"Reviews")}))      
                                    .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName:@"ProductName",outputColumnName:@"ProductName"))      
                                    .Append(mlContext.Transforms.Concatenate(@"Features", new []{@"Category",@"ProductID",@"Price",@"Rating",@"Reviews",@"ProductName"}))      
                                    .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName:@"IsPopular",inputColumnName:@"IsPopular"))      
                                    .Append(mlContext.Transforms.NormalizeMinMax(@"Features", @"Features"))      
                                    .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(new LbfgsMaximumEntropyMulticlassTrainer.Options(){L1Regularization=1F,L2Regularization=1F,LabelColumnName=@"IsPopular",FeatureColumnName=@"Features"}))      
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName:@"PredictedLabel",inputColumnName:@"PredictedLabel"));

            return pipeline;
        }
    }
}
