using HomeValueHub.AI.DependencyInjection;
using HomeValueHub.AI.UI.Extensions;
using HomeValueHub.AI.UI.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddHvhConsole(config);

            var serviceProvicer = services.BuildServiceProvider();

            Run(serviceProvicer);
        }

        static void Run(IServiceProvider serviceProvider)
        {
            var main = serviceProvider.GetRequiredService<Main>();
            main.Run();
        }
    }
}