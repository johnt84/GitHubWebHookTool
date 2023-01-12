using Amazon.Lambda.Core;
using GitHubWebHookEngine.Services.Interfaces;
using GitHubWebHookShared.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubWebHookAWSLambdaTool.Functions;

public class ReceiveFromWebHook
{
    private readonly IPushService _pushService;

    public ReceiveFromWebHook()
    {
        var startup = new Startup();
        IServiceProvider provider = startup.ConfigureServices();

        _pushService = provider.GetRequiredService<IPushService>();
    }

    [LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
    public async Task<string> FunctionHandler(PushRaw pushRaw, ILambdaContext context)
    {
        var receivePushOutput = await _pushService.ReceivePushFromWebHook(pushRaw);

        if(receivePushOutput == null)
        {
            return "Invalid Push Raw Input";
        }

        string s = string.Empty;
        string isOrAre = string.Empty;

        if ((receivePushOutput.TopicRaw?.names?.Length ?? 0) > 1)
        {
            s = "s";
            isOrAre = "are";
        }
        else
        {
            isOrAre = "is";
        }

        return $"A push occurred on GitHub repository {receivePushOutput.RepositoryName}.  The topic{s} now in the repo {isOrAre} {string.Join(", ", (receivePushOutput.TopicRaw?.names))}";
    }
}
