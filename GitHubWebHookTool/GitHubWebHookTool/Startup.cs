using GitHubWebHookTool.API;
using GitHubWebHookTool.Models;
using GitHubWebHookTool.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading;

[assembly: FunctionsStartup(typeof(GitHubWebHookTool.Startup))]
namespace GitHubWebHookTool
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory().Replace("bin\\Debug\\netcoreapp3.1", ""))
                   .AddJsonFile("appsettings.json", false)
                   .AddEnvironmentVariables()
                   .Build();

            builder.Services.Configure<ConfigurationItems>(config.GetSection("ConfigurationItems"));

            builder.Services.AddHttpClient<HttpAPIClient>();

            builder.Services.AddScoped<IPush, Push>();
            builder.Services.AddScoped<ICommit, Services.Commit>();
            builder.Services.AddScoped<ITopic, Topic>();
        }
    }
}
