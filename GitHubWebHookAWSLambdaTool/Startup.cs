using GitHubWebHookEngine.API;
using GitHubWebHookEngine.Services;
using GitHubWebHookEngine.Services.Interfaces;
using GitHubWebHookShared.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubWebHookAWSLambdaTool
{
    public static class Startup
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            //Environment Variables read from Properties/launchSettings.json

            var gitHubWebHookToolInput = new GitHubWebHookToolInput()
            {
                GitHubAPIUrl = Environment.GetEnvironmentVariable("GitHubAPIUrl"),
                PrivateToken = Environment.GetEnvironmentVariable("PrivateToken"),
            };

            services.AddSingleton(gitHubWebHookToolInput);

            services.AddHttpClient<HttpAPIClient>();

            services.AddScoped<IPushService, PushService>();
            services.AddScoped<ICommitService, CommitService>();
            services.AddScoped<ITopicService, TopicService>();

            return services.BuildServiceProvider();
        }
    }
}
