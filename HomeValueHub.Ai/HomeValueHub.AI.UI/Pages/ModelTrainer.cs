using HomeValueHub.AI.ML.Services;
using HomeValueHub.AI.Shared.Models;
using HomeValueHub.AI.Shared.Models.Settings;
using HomeValueHub.AI.UI.Helpers;
using Microsoft.Extensions.Options;

namespace HomeValueHub.AI.UI.Pages
{
    internal class ModelTrainer
    {
        private readonly EstimateModelService estimateModelService;
        private readonly string inputDataPath;

        public ModelTrainer(IOptions<EstimateModelSettings> options, EstimateModelService estimateModelService)
        {
            this.estimateModelService = estimateModelService;
            inputDataPath = options.Value.InputDataPath;
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

        public void TrainNewModel()
        {
            string? selection = string.Empty;

            while (selection != Main.quitSelection)
            {
                selection = ConsoleHelper.GetInput("use new input data file? (y/n)");

                if (selection == Main.quitSelection)
                {
                    continue;
                }

                if (selection != "y" && selection != "n")
                {
                    ConsoleHelper.ShowInvalidInputMessage();
                    continue;
                }
                else if (selection == "y")
                {
                    var key = new ConsoleKeyInfo();

                    while (key.Key != ConsoleKey.Enter)
                    {
                        Console.WriteLine($"Please drop input data file in {inputDataPath}");
                        Console.WriteLine("then press enter");

                        key = Console.ReadKey();
                    }
                }

                List<string> inputDataFiles = estimateModelService.GetExistingInputDataFileNames();

                ConsoleHelper.PrintNumberedList(inputDataFiles, "existing input data files:");

                string? inputFileSelection = string.Empty;

                while (inputFileSelection != Main.quitSelection)
                {
                    inputFileSelection = ConsoleHelper.GetInput("chose one of the existing input data files to train model");

                    if (inputFileSelection == Main.quitSelection)
                    {
                        continue;
                    }

                    if (!int.TryParse(inputFileSelection, out int dataFileId) || dataFileId > inputDataFiles.Count)
                    {
                        ConsoleHelper.ShowInvalidInputMessage();
                        continue;
                    }

                    string inputFileName = inputDataFiles[dataFileId - 1];

                    ConsoleHelper.PrintNumberedList(estimateModelService.PipelineKeys, "existing model builder pipelines:");

                    string? pipelineSelection = string.Empty;

                    while (pipelineSelection != Main.quitSelection)
                    {
                        pipelineSelection = ConsoleHelper.GetInput("select pipeline to build model");

                        if (pipelineSelection == Main.quitSelection)
                        {
                            continue;
                        }

                        if (!int.TryParse(pipelineSelection, out int pipelineId) || pipelineId > estimateModelService.PipelineKeys.Count)
                        {
                            ConsoleHelper.ShowInvalidInputMessage();
                            continue;
                        }

                        string pipelineName = estimateModelService.PipelineKeys[pipelineId - 1];

                        Console.WriteLine("training model...");

                        estimateModelService.PipelineCatalog[pipelineName](inputFileName);

                        string? evaluateSelection = string.Empty;

                        while (evaluateSelection != Main.quitSelection)
                        {
                            evaluateSelection = ConsoleHelper.GetInput("evalute model? (y/n)");

                            if (evaluateSelection == Main.quitSelection)
                            {
                                continue;
                            }

                            if (evaluateSelection != "y" && evaluateSelection != "n")
                            {
                                ConsoleHelper.ShowInvalidInputMessage();
                                continue;
                            }
                            else if (evaluateSelection == "y")
                            {
                                ModelEvaluationMetrics metrics = estimateModelService.EvaluateFastForest();

                                PrintEvaluationMetrics(metrics);
                            }

                            string? saveSelection = string.Empty;

                            while (saveSelection != Main.quitSelection)
                            {
                                saveSelection = ConsoleHelper.GetInput("save model? (y/n)");

                                if (saveSelection == Main.quitSelection)
                                {
                                    continue;
                                }

                                if (saveSelection == "y")
                                {
                                    string? modelName = string.Empty;

                                    while (modelName != Main.quitSelection)
                                    {
                                        modelName = ConsoleHelper.GetInput("enter name to save model as", true);

                                        if (modelName == Main.quitSelection)
                                        {
                                            continue;
                                        }

                                        if (string.IsNullOrWhiteSpace(modelName) || modelName.Any(c => char.IsWhiteSpace(c)))
                                        {
                                            ConsoleHelper.ShowInvalidInputMessage();
                                            continue;
                                        }

                                        estimateModelService.SaveModel(modelName);

                                        Console.WriteLine("model saved");
                                        Console.WriteLine();
                                        return;
                                    }
                                }
                                else if (saveSelection == "n")
                                {
                                    Console.WriteLine("exiting without saving model...");
                                    Console.WriteLine();
                                    return;
                                }
                                else
                                {
                                    ConsoleHelper.ShowInvalidInputMessage();
                                    continue;
                                }
                            }
                        }
                    }
                }
            }




            // prompt selection

            // chose pipeline from catelog
            // run pipeline
            // prompt evaluate?
            // if yest evaluate
            // prompt save?
            // if yes prompt name
            // return to main
        }
    }
}
