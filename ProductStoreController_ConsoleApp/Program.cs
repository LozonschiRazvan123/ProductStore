﻿
// This file was auto-generated by ML.NET Model Builder. 

using ProductStoreController_ConsoleApp;

// Create single instance of sample data from first line of dataset for model input
ProductStoreController.ModelInput sampleData = new ProductStoreController.ModelInput()
{
    ProductID = 2F,
    ProductName = @"Smartphone",
    Price = 800F,
    Category = @"Electronics",
    Rating = 4.2F,
    Reviews = 90F,
};

// Make a single prediction on the sample data and print results
var predictionResult = ProductStoreController.Predict(sampleData);

Console.WriteLine("Using model to make single prediction -- Comparing actual IsPopular with predicted IsPopular from sample data...\n\n");


Console.WriteLine($"ProductID: {2F}");
Console.WriteLine($"ProductName: {@"Smartphone"}");
Console.WriteLine($"Price: {800F}");
Console.WriteLine($"Category: {@"Electronics"}");
Console.WriteLine($"IsPopular: {1F}");
Console.WriteLine($"Rating: {4.2F}");
Console.WriteLine($"Reviews: {90F}");


Console.WriteLine($"\n\nPredicted IsPopular: {predictionResult.PredictedLabel}\n\n");
Console.WriteLine("=============== End of process, hit any key to finish ===============");
Console.ReadKey();
