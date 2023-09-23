using HomeValueHub.AI.ML.Services;
using HomeValueHub.AI.Shared.Models;
using HomeValueHub.Shared.Models;

namespace HomeValueHub.API.Services
{
    public class EstimateService
    {
        private readonly EstimateModelService estimateModelService;

        public EstimateService(EstimateModelService estimateModelService)
        {
            this.estimateModelService = estimateModelService;
            estimateModelService.LoadModel("HvhEstimateModel");
        }

        public HomeValueEstimate GetEstimate(HomeDetail homeDetail)
        {
            var estimate = Map(estimateModelService.GetEstimate(Map(homeDetail)));

            return estimate;
        }

        private EstimateInput Map(HomeDetail homeDetail)
        {
            return new EstimateInput
            {
                Bathrooms = homeDetail.Features.Bathrooms,
                Bedrooms = homeDetail.Features.Bedrooms,
                FinishedSquareFeet = (float)homeDetail.Features.FinishedSquareFeet,
                TotalRooms = homeDetail.Features.TotalRooms
            };
        }

        private HomeValueEstimate Map(EstimateOutput estimateOutput)
        {
            return new HomeValueEstimate 
            {
                SalePrice = (decimal)estimateOutput.Price
            };
        }
    }
}
