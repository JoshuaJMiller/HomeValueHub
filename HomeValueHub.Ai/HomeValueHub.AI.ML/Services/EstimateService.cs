using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeValueHub.AI.Shared.Models;
using HomeValueHub.AI.Shared.Models.Settings;
using Microsoft.Extensions.ML;
using Microsoft.Extensions.Options;
using Microsoft.ML;

namespace HomeValueHub.AI.ML.Services
{
    public class EstimateService
    {
        private readonly IOptions<EstimateSettings> options;
        private PredictionEngine<EstimateInput, EstimateOutput> predictionEngine;

        //private readonly PredictionEnginePool<EstimateInput, EstimateOutput> predictionEnginePool;

        public EstimateService(IOptions<EstimateSettings> options)
        {
            this.options = options;
        }

        private void LoadModel()
        {
            if (File.Exists(options.Value.ModelPath))
            {
                var context = new MLContext();

                var model = context.Model.Load(options.Value.ModelPath, out _);

                predictionEngine = context.Model.CreatePredictionEngine<EstimateInput, EstimateOutput>(model);
            }
        }

        public void Foo()
        {
            if (predictionEngine == null)
            {
                LoadModel();
            }

            var input = new EstimateInput
            {
                Bathrooms = 1f,
                Bedrooms = 1f,
                TotalRooms = 3f,
                FinishedSquareFeet = 653f,
                UseCode = "Condominium",
                LastSoldPrice = 0f,
            };

            var output = predictionEngine.Predict(input);

            Console.WriteLine(output.Price);
        }
    }
}
