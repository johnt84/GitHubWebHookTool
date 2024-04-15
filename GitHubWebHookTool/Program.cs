using GitHubWebHookEngine.API;
using GitHubWebHookEngine.Services;
using GitHubWebHookEngine.Services.Interfaces;
using GitHubWebHookShared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(builder => builder.AddUserSecrets(Assembly.GetExecutingAssembly(), true))
    .ConfigureServices((services) => Configure(services))
    .Build();

host.Run();

public sealed partial class Program
{
    public static void Configure(IServiceCollection services)
    {
        // Environment Variables read from local.settings.json

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
    }
}