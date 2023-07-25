using HomeValueHub.AI.UI.Models;
using HomeValueHub.AI.UI.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeValueHub.AI.UI.Extensions
{
    public static class HvhAiUiExtensions
    {
        public static void AddHvhConsole(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<HvhConsoleSettings>(configuration.GetSection(nameof(HvhConsoleSettings)));
            serviceCollection.AddSingleton<Main>();
            serviceCollection.AddSingleton<ModelTrainer>();
            serviceCollection.AddSingleton<ManualEstimator>();
        }
    }
}
