using HomeValueHub.Shared.Models;

namespace HomeValueHub.Client.Pages
{
    public partial class Estimate
    {
        public HomeFeatures HomeFeatures { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            HomeFeatures = new HomeFeatures();
        }

        public async Task HandleValidSubmit()
        {
            await Console.Out.WriteLineAsync("Submitting! :)");
        }
    }
}
