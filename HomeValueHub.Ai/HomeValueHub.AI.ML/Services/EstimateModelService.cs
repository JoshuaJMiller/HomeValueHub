using HomeValueHub.AI.Shared.Models;
using HomeValueHub.AI.Shared.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.ML;

namespace HomeValueHub.AI.ML.Services
{
    public class EstimateModelService
    {
        private ITransformer transformer;
        private PredictionEngine<EstimateInput, EstimateOutput> predictionEngine;
        private readonly MLContext context;

        private readonly string inputDataPath;
        private readonly string modelPath;
        private readonly string modelFileExtension;

        public bool ModelExists => Directory.EnumerateFileSystemEntries(modelPath).Any();

        public EstimateModelService(IOptions<EstimateModelSettings> options, MLContext context)
        {
            inputDataPath = options.Value.InputDataPath;
            modelPath = options.Value.ModelPath;
            modelFileExtension = options.Value.ModelFileExtension;
            this.context = context;
        }

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
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }

            context.Model.Save(model, trainData.Schema, $"{modelPath}/HvhEstimateModel.zip");

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

        private void LoadModel(string modelName = null)
        {
            string fullPath = string.Empty;

            if (!string.IsNullOrWhiteSpace(modelName))
            {
                fullPath = $"{modelPath}/{modelName}";
            }
            else
            {
                fullPath = $"{modelPath}/{GetExistingModelList().First()}";
            }

            transformer = context.Model.Load(fullPath, out _);
        }

        public void Foo()
        {
            if (predictionEngine == null)
            {
                LoadModel();
            }

            var input = new EstimateInput
            {
                Bathrooms = 1f,
                Bedrooms = 1f,
                TotalRooms = 3f,
                FinishedSquareFeet = 653f,
                UseCode = "Condominium",
                LastSoldPrice = 0f,
            };

            var output = predictionEngine.Predict(input);

            Console.WriteLine(output.Price);
        }

        public List<string> GetExistingModelList()
        {
            var results = new List<string>();

            var directoryInfo = new DirectoryInfo(modelPath);
            FileInfo[] files = directoryInfo.GetFiles($"*{modelFileExtension}");

            foreach (var file in files)
            {
                results.Add(file.Name);
            }

            return results;
        }

        public List<string> GetExistingInputDataFileNames()
        {
            var results = new List<string>();

            var directoryInfo = new DirectoryInfo(inputDataPath);
            FileInfo[] files = directoryInfo.GetFiles();

            foreach (var file in files)
            {
                results.Add(file.Name);
            }

            return results;
        }

        public ModelEvaluationMetrics Evaluate(string modelName, InputFileDetails inputFileDetails)
        {
            LoadModel(modelName);

            var data = context.Data.LoadFromTextFile<EstimateInput>($"{inputDataPath}/{inputFileDetails.FileName}", hasHeader: inputFileDetails.HasHeader, separatorChar: inputFileDetails.Separator);

            // TODO: impl real evaluation

            return new ModelEvaluationMetrics
            {
                RSquared = 0
            };

            //predictionEngine = context.Model.CreatePredictionEngine<EstimateInput, EstimateOutput>(transformer);
        }
    }
}
