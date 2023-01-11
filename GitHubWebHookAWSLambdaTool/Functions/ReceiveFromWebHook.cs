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
    public async Task<ReceivePushOutput> FunctionHandler(PushRaw pushRaw, ILambdaContext context)
    {
        return await _pushService.ReceivePushFromWebHook(pushRaw);
    }
}
