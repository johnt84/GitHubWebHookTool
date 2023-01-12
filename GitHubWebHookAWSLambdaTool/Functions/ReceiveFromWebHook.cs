using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using GitHubWebHookEngine.Services.Interfaces;
using GitHubWebHookShared.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;

namespace GitHubWebHookAWSLambdaTool.Functions;

public class ReceiveFromWebHook
{
    private readonly IPushService _pushService;

    public ReceiveFromWebHook()
    {
        IServiceProvider provider = Startup.ConfigureServices();

        _pushService = provider.GetRequiredService<IPushService>();
    }

    [LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        context.Logger.LogLine($"ReceiveFromWebHook received a push request pushRaw: {request.Body}");

        if (string.IsNullOrWhiteSpace(request.Body))
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = "Invalid Push Raw Input",
            };
        }

        var pushRaw = JsonConvert.DeserializeObject<PushRaw>(request.Body);

        var receivePushOutput = await _pushService.ReceivePushFromWebHook(pushRaw);

        context.Logger.LogLine($"\nReceivePushOutput returned from the PushService: {JsonConvert.SerializeObject(receivePushOutput)}");

        if (receivePushOutput == null)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = "Invalid Push Raw Input",
            };
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

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = $"A push occurred on GitHub repository {receivePushOutput.RepositoryName}.  The topic{s} now in the repo {isOrAre} {string.Join(", ", (receivePushOutput.TopicRaw?.names))}",
        };
    }
}
