using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HomeValueHub.AI.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace HomeValueHub.AI.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var services = new ServiceCollection();
            services.AddHvhEstimator(config);
            services.AddSingleton<App>();

            var serviceProvicer = services.BuildServiceProvider();

            Run(serviceProvicer);
        }

        static void Run(IServiceProvider serviceProvider)
        {
            var app = serviceProvider.GetRequiredService<App>();
            app.Run();
        }
    }
}