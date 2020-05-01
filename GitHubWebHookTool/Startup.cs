using GitHubWebHookTool.API;
using GitHubWebHookTool.Services;
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

            builder.Services.AddScoped<IPush, Push>();
            builder.Services.AddScoped<ICommit, Services.Commit>();
            builder.Services.AddScoped<ITopic, Topic>();
        }
    }
}
