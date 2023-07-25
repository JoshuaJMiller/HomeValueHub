using HomeValueHub.AI.UI.Helpers;

namespace HomeValueHub.AI.UI.Pages
{
    internal class ManualEstimator
    {
        internal void Run()
        {
            string? selection = string.Empty;

            while (selection != Main.quitSelection)
            {
                selection = RunManualEstimateMenu();

                switch (selection)
                {
                    case "a":
                        Console.WriteLine("toilet");
                        break;
                    case "b":
                        Console.WriteLine("toilet");
                        break;
                    case "c":
                        Console.WriteLine("toilet");
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
            Console.WriteLine("manual estimate options...");
            Console.WriteLine();
            return ConsoleHelper.GetInput();
        }
    }
}
