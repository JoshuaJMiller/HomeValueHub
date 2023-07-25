namespace HomeValueHub.AI.UI.Helpers
{
    internal static class ConsoleHelper
    {
        internal static string? GetInput(string? prompt = null, bool caseSenstive = false)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                prompt = "> ";
            }
            else
            {
                prompt = $"{prompt} > ";
            }

            Console.Write(prompt);

            string? response = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(response))
            {
                Console.WriteLine();
                return caseSenstive ? response.Trim() : response.Trim().ToLower();
            }

            Console.WriteLine();
            return null;
        }

        internal static void ShowInvalidInputMessage()
        {
            Console.WriteLine("invalid input. . .");
            Console.WriteLine();
        }

        internal static void PrintNumberedList(List<string> list, string? caption = null, bool zeroBased = false)
        {
            if (!string.IsNullOrEmpty(caption))
            {
                Console.WriteLine(caption);
            }

            for (int i = 0; i < list.Count; i++)
            {
                int listNumber = zeroBased ? i : i + 1;

                Console.WriteLine($"\t{listNumber} - {list[i]}");
            }

            Console.WriteLine();
        }
    }
}
