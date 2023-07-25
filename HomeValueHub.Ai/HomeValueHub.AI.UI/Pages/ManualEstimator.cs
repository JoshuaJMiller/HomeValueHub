using HomeValueHub.AI.ML.Services;
using HomeValueHub.AI.Shared.Models;
using HomeValueHub.AI.UI.Helpers;

namespace HomeValueHub.AI.UI.Pages
{
    internal class ManualEstimator
    {
        private readonly EstimateModelService estimateModelService;
        private readonly ModelTrainer modelTrainer;

        public ManualEstimator(EstimateModelService estimateModelService, ModelTrainer modelTrainer)
        {
            this.estimateModelService = estimateModelService;
            this.modelTrainer = modelTrainer;
        }

        internal void Run()
        {
            string? selection = string.Empty;

            while (selection != Main.quitSelection)
            {
                selection = RunManualEstimateMenu();

                switch (selection)
                {
                    case "e":
                        EstimateWithExistingModel();
                        break;
                    case "n":
                        modelTrainer.TrainNewModel();
                        EstimateWithExistingModel();
                        break;
                    case Main.quitSelection:
                        break;
                    default:
                        ConsoleHelper.ShowInvalidInputMessage();
                        break;
                }
            }
        }

        private string? RunManualEstimateMenu()
        {
            Console.WriteLine("Manual Estimator:");
            Console.WriteLine("\te - manually estimate using (e)xisting model");
            Console.WriteLine("\tn - manually estimate with (n)ew model");
            Console.WriteLine();
            return ConsoleHelper.GetInput();
        }

        private void EstimateWithExistingModel()
        {
            List<string> existingModels = estimateModelService.GetExistingModelList();

            ConsoleHelper.PrintNumberedList(existingModels, "existing models:");

            string? modelSelection = string.Empty;

            while (modelSelection != Main.quitSelection)
            {
                modelSelection = ConsoleHelper.GetInput("select model to get manual estimate");

                if (modelSelection == Main.quitSelection)
                {
                    continue;
                }

                if (!int.TryParse(modelSelection, out int modelId) || modelId > existingModels.Count)
                {
                    ConsoleHelper.ShowInvalidInputMessage();
                    continue;
                }

                string modelName = existingModels[modelId - 1];

                var input = new EstimateInput();

                string? bedrooms = string.Empty;
                string? bathrooms = string.Empty;
                string? finishedSquareFeet = string.Empty;
                string? totalRooms = string.Empty;

                while (bedrooms != Main.quitSelection)
                {
                    bedrooms = ConsoleHelper.GetInput("enter number of bedrooms");

                    if (bedrooms == Main.quitSelection)
                    {
                        continue;
                    }

                    if (!int.TryParse(bedrooms, out int bedsNum) || bedsNum < 1)
                    {
                        ConsoleHelper.ShowInvalidInputMessage();
                        continue;
                    }

                    input.Bedrooms = bedsNum;

                    while (bathrooms != Main.quitSelection)
                    {
                        bathrooms = ConsoleHelper.GetInput("enter number of bathrooms");

                        if (bedrooms == Main.quitSelection)
                        {
                            continue;
                        }

                        if (!int.TryParse(bathrooms, out int bathsNum) || bathsNum < 1)
                        {
                            ConsoleHelper.ShowInvalidInputMessage();
                            continue;
                        }

                        input.Bathrooms = bathsNum;

                        while (finishedSquareFeet != Main.quitSelection)
                        {
                            finishedSquareFeet = ConsoleHelper.GetInput("enter finished square feet");

                            if (finishedSquareFeet == Main.quitSelection)
                            {
                                continue;
                            }

                            if (!int.TryParse(finishedSquareFeet, out int finishedFeetNum) || finishedFeetNum < 1)
                            {
                                ConsoleHelper.ShowInvalidInputMessage();
                                continue;
                            }

                            input.FinishedSquareFeet = finishedFeetNum;

                            while (totalRooms != Main.quitSelection)
                            {
                                totalRooms = ConsoleHelper.GetInput("enter total number of rooms");

                                if (totalRooms == Main.quitSelection)
                                {
                                    continue;
                                }

                                if (!int.TryParse(totalRooms, out int totalRoomsNum) || totalRoomsNum < 1)
                                {
                                    ConsoleHelper.ShowInvalidInputMessage();
                                    continue;
                                }

                                input.TotalRooms = totalRoomsNum;

                                Console.WriteLine("getting estimate...\n");

                                EstimateOutput estimate = estimateModelService.RunManualEstimate(input, modelName);

                                Console.WriteLine($"estimated price: {estimate.Price:c}");
                                Console.WriteLine();
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
