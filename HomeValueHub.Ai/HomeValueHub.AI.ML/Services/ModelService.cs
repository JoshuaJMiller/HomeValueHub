using HomeValueHub.AI.Shared.Models;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeValueHub.AI.ML.Services
{
    public class ModelService
    {
        private string inputDataPath = "../../../../HomeValueHub.AI.ML/InputData/pacific-heights.csv";
        private string outputModelPath = "../../../../HomeValueHub.AI.ML/Models";

        public void BuildTrainBlahBlah()
        {
            // create context
            var context = new MLContext(seed: 0);

            // load data
            var data = context.Data.LoadFromTextFile<EstimateInput>(inputDataPath, hasHeader: true, separatorChar: ',');

            // split the data into training and testing
            var trainingTestData = context.Data.TrainTestSplit(data, testFraction: 0.2, seed: 0);
            var trainData = trainingTestData.TrainSet;
            var testData = trainingTestData.TestSet;

            // build and train the model
            var pipeline = context.Transforms.Categorical.OneHotEncoding(inputColumnName: "UseCode", outputColumnName: "UseCodeEncoded")
                .Append(context.Transforms.Concatenate("Features", "UseCodeEncoded", "Bathrooms", "Bedrooms", "FinishedSquareFeet", "TotalRooms")
                .Append(context.Regression.Trainers.FastForest(numberOfTrees: 200, minimumExampleCountPerLeaf: 4)));

            var model = pipeline.Fit(trainData);

            // evaluate the model
            var scores = context.Regression.CrossValidate(data, pipeline, numberOfFolds: 5);
            var mean = scores.Average(x => x.Metrics.RSquared);

            Console.WriteLine($"Mean cross-validated R2 score: {mean:0.##}");

            // save model
            if (!Directory.Exists(outputModelPath))
            {
                Directory.CreateDirectory(outputModelPath);
            }

            context.Model.Save(model, trainData.Schema, $"{outputModelPath}/HvhEstimateModel.zip");

            // use the model
            var input = new EstimateInput
            {
                Bathrooms = 1f,
                Bedrooms = 1f,
                TotalRooms = 3f,
                FinishedSquareFeet = 653f,
                UseCode = "Condominium",
                LastSoldPrice = 0f,
            };

            var engine = context.Model.CreatePredictionEngine<EstimateInput, EstimateOutput>(model);

            Console.WriteLine($"Price: {engine.Predict(input).Price:c}");
        }
    }
}
