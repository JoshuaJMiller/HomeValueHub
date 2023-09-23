using HomeValueHub.AI.Shared.Models;
using HomeValueHub.AI.Shared.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.ML;

namespace HomeValueHub.AI.ML.Services
{
    public class EstimateModelService
    {
        private ITransformer? transformer;
        private IDataView? dataView;
        private IEstimator<ITransformer>? pipeline;

        private readonly MLContext context;
        private readonly string inputDataPath;
        private readonly string modelPath;
        private readonly string modelFileExtension;

        public bool ModelExists => Directory.EnumerateFileSystemEntries(modelPath).Any();
        public Dictionary<string, Action<string>> PipelineCatalog { get; set; }
        public List<string> PipelineKeys => PipelineCatalog.Select(p => p.Key.ToString()).ToList();

        public EstimateModelService(IOptions<EstimateModelSettings> options, MLContext context)
        {
            inputDataPath = options.Value.InputDataPath;
            modelPath = options.Value.ModelPath;
            modelFileExtension = options.Value.ModelFileExtension;
            this.context = context;

            PipelineCatalog = new Dictionary<string, Action<string>>
            {
                { "FastForest1", (x) => RunFastForest1(x) },
                { "FastForest2", (x) => RunFastForest2(x) }
            };
        }

        public void RunFastForest1(string inputFileName)
        {
            ResetState();

            // load data
            dataView = context.Data.LoadFromTextFile<EstimateInput>($"{inputDataPath}/{inputFileName}", hasHeader: true, separatorChar: ',');

            // split the data into training and testing
            var trainingTestData = context.Data.TrainTestSplit(dataView, testFraction: 0.2, seed: 0);
            var trainData = trainingTestData.TrainSet;

            // build and train the model
            pipeline = context.Transforms.Categorical.OneHotEncoding(inputColumnName: "UseCode", outputColumnName: "UseCodeEncoded")
                .Append(context.Transforms.Concatenate("Features", "UseCodeEncoded", "Bathrooms", "Bedrooms", "FinishedSquareFeet", "TotalRooms")
                .Append(context.Regression.Trainers.FastForest(numberOfTrees: 200, minimumExampleCountPerLeaf: 4)));

            transformer = pipeline.Fit(trainData);
        }

        public void RunFastForest2(string inputFileName)
        {
            ResetState();

            // load data
            dataView = context.Data.LoadFromTextFile<EstimateInput>($"{inputDataPath}/{inputFileName}", hasHeader: true, separatorChar: ',');

            // split the data into training and testing
            var trainingTestData = context.Data.TrainTestSplit(dataView, testFraction: 0.2, seed: 0);
            var trainData = trainingTestData.TrainSet;

            // build and train the model
            pipeline = context.Transforms.Categorical.OneHotEncoding(inputColumnName: "UseCode", outputColumnName: "UseCodeEncoded")
                .Append(context.Transforms.Concatenate("Features", "UseCodeEncoded", "Bathrooms", "Bedrooms", "FinishedSquareFeet", "TotalRooms")
                .Append(context.Regression.Trainers.FastForest(numberOfTrees: 300, minimumExampleCountPerLeaf: 6)));

            transformer = pipeline.Fit(trainData);
        }

        public ModelEvaluationMetrics EvaluateFastForest()
        {
            // evaluate the model
            var scores = context.Regression.CrossValidate(dataView, pipeline, numberOfFolds: 5);

            return new ModelEvaluationMetrics
            {
                RSquared = scores.Average(x => x.Metrics.RSquared)
            };
        }

        public void SaveModel(string name)
        {
            string cleanedName = Path.GetFileNameWithoutExtension(name);

            // save model
            if (!Directory.Exists(modelPath))
            {
                Directory.CreateDirectory(modelPath);
            }

            context.Model.Save(transformer, dataView.Schema, $"{modelPath}/{cleanedName}.zip");
        }

        public void LoadModel(string modelName = null)
        {
            string fullPath;

            if (!string.IsNullOrWhiteSpace(modelName))
            {
                fullPath = $"{modelPath}/{modelName}{modelFileExtension}";
            }
            else
            {
                fullPath = $"{modelPath}/{GetExistingModelList().First()}";
            }

            transformer = context.Model.Load(fullPath, out _);
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
                RSquared = 0 // replace this with real eval metric...
            };
        }

        public EstimateOutput RunManualEstimate(EstimateInput input, string modelName)
        {
            ResetState();

            LoadModel(modelName);

            return GetEstimate(input);
        }

        public EstimateOutput GetEstimate(EstimateInput estimateInput)
        {
            var engine = context.Model.CreatePredictionEngine<EstimateInput, EstimateOutput>(transformer);

            return engine.Predict(estimateInput); 
        }

        private void ResetState()
        {
            // don't love this... but keeping this service stateful works well for now
            transformer = null;
            dataView = null;
            pipeline = null;
        }
    }
}
