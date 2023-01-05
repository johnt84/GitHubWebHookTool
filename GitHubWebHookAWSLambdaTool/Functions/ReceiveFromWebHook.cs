using Amazon.Lambda.Core;
using GitHubWebHookEngine.Services.Interfaces;

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
    public string FunctionHandler(string input, ILambdaContext context)
    {
        return input.ToUpper();
    }
}
