using HomeValueHub.AI.ML.Services;
using HomeValueHub.AI.Shared.Models;
using HomeValueHub.AI.UI.Helpers;

namespace HomeValueHub.AI.UI.Pages
{
    internal class ModelTrainer
    {
        private readonly EstimateModelService estimateModelService;

        public ModelTrainer(EstimateModelService estimateModelService)
        {
            this.estimateModelService = estimateModelService;
        }

        internal void Run()
        {
            string? selection = string.Empty;

            while (selection != Main.quitSelection)
            {
                selection = RunModelTrainerMenu();

                switch (selection)
                {
                    case "m":
                        ViewExistingModels();
                        break;
                    case "e":
                        EvaluateExistingModel();
                        break;
                    case "n":
                        TrainNewModel();
                        break;
                    case Main.quitSelection:
                        break;
                    default:
                        ConsoleHelper.ShowInvalidInputMessage();
                        break;
                }
            }
        }

        private string? RunModelTrainerMenu()
        {
            Console.WriteLine("Model Trainer:");
            Console.WriteLine("\tm - view existing (m)odels");
            Console.WriteLine("\te - (e)valuate existing model");
            Console.WriteLine("\tn - train (n)ew model");
            Console.WriteLine();
            return ConsoleHelper.GetInput();
        }

        private void ViewExistingModels()
        {
            List<string> extistingModels = estimateModelService.GetExistingModelList();

            ConsoleHelper.PrintNumberedList(extistingModels, "existing models:");
        }

        private void EvaluateExistingModel()
        {
            List<string> extistingModels = estimateModelService.GetExistingModelList();

            ConsoleHelper.PrintNumberedList(extistingModels, "existing models:");

            string? modelSelection = string.Empty;

            while (modelSelection != Main.quitSelection)
            {
                modelSelection = ConsoleHelper.GetInput("chose one of the existing models to evaluate");

                if (modelSelection == Main.quitSelection)
                {
                    continue;
                }

                if (!int.TryParse(modelSelection, out int modelId) || modelId > extistingModels.Count)
                {
                    ConsoleHelper.ShowInvalidInputMessage();
                    continue;
                }

                string modelName = extistingModels[modelId - 1];

                List<string> inputDataFiles = estimateModelService.GetExistingInputDataFileNames();

                ConsoleHelper.PrintNumberedList(inputDataFiles, "existing input data files:");

                string? inputFileSelection = string.Empty;

                while (inputFileSelection != Main.quitSelection)
                {
                    inputFileSelection = ConsoleHelper.GetInput("chose one of the existing input data files to use in evaluation");

                    if (inputFileSelection == Main.quitSelection)
                    {
                        continue;
                    }

                    if (!int.TryParse(inputFileSelection, out int dataFileId) || dataFileId > inputDataFiles.Count)
                    {
                        ConsoleHelper.ShowInvalidInputMessage();
                        continue;
                    }

                    var fileDetails = new InputFileDetails
                    {
                        FileName = inputDataFiles[dataFileId - 1],
                        HasHeader = true,
                        Separator = ','
                    };

                    ModelEvaluationMetrics metrics = estimateModelService.Evaluate(modelName, fileDetails);

                    PrintEvaluationMetrics(metrics, modelName);
                }
            }
        }

        private void PrintEvaluationMetrics(ModelEvaluationMetrics metrics, string? modelName = null)
        {
            string caption = "model metrics";

            if (modelName != null)
            {
                caption = $"{caption} for {modelName}";
            }

            caption += ':';

            Console.WriteLine(caption);

            Console.WriteLine($"\tMean cross-validated R2 score: {metrics.RSquared:0.##}");

            Console.WriteLine();
        }

        private void TrainNewModel()
        {
            Console.WriteLine("running TrainNewModel");
            Console.WriteLine();
        }
    }
}
