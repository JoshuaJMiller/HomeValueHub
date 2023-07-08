using HomeValueHub.Shared.Models;

namespace HomeValueHub.Client.Pages
{
    public partial class Estimate
    {
        public HomeDetails HomeDetails { get; set; }
        private int currentStep;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            HomeDetails = new HomeDetails();

            currentStep = 1;
        }

        public async Task HandleValidNext()
        {
            currentStep++;
        }

        public async Task HandleValidSubmit()
        {
            await Console.Out.WriteLineAsync("Submitting! :)");
        }
    }
}
