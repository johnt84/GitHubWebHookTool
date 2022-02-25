using GitHubWebHookTool.API;
using GitHubWebHookTool.Services;
using GitHubWebHookTool.Services.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(GitHubWebHookTool.Startup))]
namespace GitHubWebHookTool
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<HttpAPIClient>();

            builder.Services.AddScoped<IPushService, PushService>();
            builder.Services.AddScoped<ICommitService, CommitService>();
            builder.Services.AddScoped<ITopicService, TopicService>();
        }
    }
}
