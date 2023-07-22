using HomeValueHub.AI.ML.Services;

namespace HomeValueHub.AI.UI
{
    internal class App
    {
        private readonly ModelService modelService;
        private readonly EstimateService estimateService;

        public App(ModelService modelService, EstimateService estimateService)
        {
            this.modelService = modelService;
            this.estimateService = estimateService;
        }

        internal void Run()
        {
            Console.WriteLine("RUNNING!");
            //modelService.BuildTrainBlahBlah();
            estimateService.Foo();
            estimateService.Foo();
        }
    }
}
