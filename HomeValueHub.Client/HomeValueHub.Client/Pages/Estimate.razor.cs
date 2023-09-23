using HomeValueHub.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HomeValueHub.Client.Pages
{
    public partial class Estimate
    {
        [Inject]
        IHttpClientFactory httpClientFactory { get; set; }

        public HomeDetail HomeDetail { get; set; }
        public HomeValueEstimate HomeValueEstimate { get; set; }
        
        private int currentStep;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            HomeDetail = new HomeDetail();
            HomeValueEstimate = new HomeValueEstimate();

            currentStep = 1;
        }

        public async Task HandleValidNext()
        {
            currentStep++;
        }

        public async Task HandleValidSubmit()
        {
            await Console.Out.WriteLineAsync("Submitting! :)");

            await GetEstimate();

            await Console.Out.WriteLineAsync(HomeValueEstimate.SalePrice.ToString());
        }

        private async Task GetEstimate()
        {
            await Console.Out.WriteLineAsync("calling API!");

            var client = httpClientFactory.CreateClient("hvh");

            var result = await client.PostAsJsonAsync("api/estimate", HomeDetail);

            string json = await result.Content.ReadAsStringAsync();

            await Console.Out.WriteLineAsync(json);

            HomeValueEstimate = JsonSerializer.Deserialize<HomeValueEstimate>(json);
        }
    }
}
