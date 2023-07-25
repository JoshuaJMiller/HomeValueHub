using HomeValueHub.AI.ML.Services;
using HomeValueHub.AI.Shared.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;

namespace HomeValueHub.AI.DependencyInjection
{
    public static class HvhAiExtensions
    {
        public static void AddHvhEstimator(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<EstimateModelSettings>(configuration.GetSection(nameof(EstimateModelSettings)));
            serviceCollection.AddSingleton<EstimateModelService>();
            serviceCollection.AddSingleton<MLContext>();
        }
    }
}
