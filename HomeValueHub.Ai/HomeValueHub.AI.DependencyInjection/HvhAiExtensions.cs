using HomeValueHub.AI.ML.Services;
using HomeValueHub.AI.Shared.Models;
using HomeValueHub.AI.Shared.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;

namespace HomeValueHub.AI.DependencyInjection
{
    public static class HvhAiExtensions
    {
        public static void AddHvhEstimator(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //string modelPath = configuration.GetValue<string>("ModelPath");

            serviceCollection.Configure<EstimateSettings>(configuration.GetSection(nameof(EstimateSettings)));

            serviceCollection.AddSingleton<ModelService>();
            //serviceCollection.AddPredictionEnginePool<EstimateInput, EstimateOutput>()
            //    .FromFile(modelPath, true);
            serviceCollection.AddSingleton<EstimateService>();
        }
    }
}
