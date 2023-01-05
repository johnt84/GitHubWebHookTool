using GitHubWebHookEngine.Services;
using GitHubWebHookEngine.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubWebHookAWSLambdaTool
{
    public class Startup
    {
        private readonly IConfigurationRoot Configuration;

        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                                .AddEnvironmentVariables()
                                .Build();
        }

        public IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IPushService, PushService>();

            IServiceProvider provider = services.BuildServiceProvider();

            return provider;
        }
    }
}
