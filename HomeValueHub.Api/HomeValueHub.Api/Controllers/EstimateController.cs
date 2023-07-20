using HomeValueHub.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeValueHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstimateController : ControllerBase
    {
        [HttpPost]
        public async Task<HomeValueEstimate> GetEstimate(HomeDetail homeDetail)
        {
            return new HomeValueEstimate
            {
                SalePrice = 999999.999M
            };
        }
    }
}
