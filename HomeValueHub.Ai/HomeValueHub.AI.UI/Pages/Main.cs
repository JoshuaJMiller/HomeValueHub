using HomeValueHub.AI.UI.Helpers;
using HomeValueHub.AI.UI.Models;
using Microsoft.Extensions.Options;

namespace HomeValueHub.AI.UI.Pages
{
    internal class Main
    {
        private readonly ModelTrainer modelTrainer;
        private readonly ManualEstimator manualEstimator;
        private readonly string title;

        public const string quitSelection = "quit";

        public Main(IOptions<HvhConsoleSettings> options, ModelTrainer modelTrainer, ManualEstimator manualEstimator)
        {
            title = options.Value.Title;
            this.modelTrainer = modelTrainer;
            this.manualEstimator = manualEstimator;
        }

        internal void Run()
        {
            ShowTitle();

            while (true)
            {
                string? selection = RunMainMenu();

                switch (selection)
                {
                    case "t":
                        modelTrainer.Run();
                        break;
                    case "m":
                        manualEstimator.Run();
                        break;
                    default:
                        ConsoleHelper.ShowInvalidInputMessage();
                        break;
                }
            }
        }

        private void ShowTitle()
        {
            Console.SetCursorPosition((Console.WindowWidth - title.Length) / 2, Console.CursorTop);
            Console.WriteLine(title);
            Console.WriteLine();
        }

        private string? RunMainMenu()
        {
            Console.WriteLine("Main Menu:");
            Console.WriteLine("\tt - run model (t)rainer");
            Console.WriteLine("\tm - run (m)anual estimate");
            Console.WriteLine();
            return ConsoleHelper.GetInput();
        }
    }
}
