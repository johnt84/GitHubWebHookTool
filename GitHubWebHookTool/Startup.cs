using GitHubWebHookEngine.API;
using GitHubWebHookEngine.Services;
using GitHubWebHookEngine.Services.Interfaces;
using GitHubWebHookShared.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(GitHubWebHookTool.Startup))]
namespace GitHubWebHookTool
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //Environment Variables read from local.settings.json

            var gitHubWebHookToolInput = new GitHubWebHookToolInput()
            {
                GitHubAPIUrl = Environment.GetEnvironmentVariable("GitHubAPIUrl"),
                PrivateToken = Environment.GetEnvironmentVariable("PrivateToken"),
            };

            builder.Services.AddSingleton(gitHubWebHookToolInput);

            builder.Services.AddHttpClient<HttpAPIClient>();

            builder.Services.AddScoped<IPushService, PushService>();
            builder.Services.AddScoped<ICommitService, CommitService>();
            builder.Services.AddScoped<ITopicService, TopicService>();
        }
    }
}
