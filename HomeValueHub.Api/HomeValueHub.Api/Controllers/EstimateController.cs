using HomeValueHub.API.Services;
using HomeValueHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeValueHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstimateController : ControllerBase
    {
        private readonly EstimateService estimateService;

        public EstimateController(EstimateService estimateService)
        {
            this.estimateService = estimateService;
        }

        [HttpPost]
        public async Task<HomeValueEstimate> GetEstimate(HomeDetail homeDetail)
        {
            // TODO: handle http stuff...
            return estimateService.GetEstimate(homeDetail);
        }
    }
}
