using GitHubWebHookEngine.Services.Interfaces;
using GitHubWebHookShared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace GitHubWebHookTool;

public class ReceiveFromWebHook
{
    private readonly ILogger _logger;
    private readonly IPushService _pushService;

    public ReceiveFromWebHook(ILogger<ReceiveFromWebHook> logger, IPushService pushService)
    {
        _logger = logger;
        _pushService = pushService;
    }

    [Function("ReceiveFromWebHook")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ExecutionContext context)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        _logger.LogInformation($"ReceiveFromWebHook received a push request pushRaw: {requestBody}");

        if (string.IsNullOrWhiteSpace(requestBody))
        {
            return (ActionResult)new BadRequestObjectResult("Invalid Push Raw Input");
        }

        var pushRaw = JsonConvert.DeserializeObject<PushRaw>(requestBody);

        var receivePushOutput = await _pushService.ReceivePushFromWebHook(pushRaw);

        _logger.LogInformation($"\nReceivePushOutput returned from the PushService: {JsonConvert.SerializeObject(receivePushOutput)}");

        if (receivePushOutput == null)
        {
            return (ActionResult)new BadRequestObjectResult("Invalid Push Raw Input");
        }

        string s = string.Empty;
        string isOrAre = string.Empty;

        if (receivePushOutput.TopicRaw?.names.Length > 1)
        {
            s = "s";
            isOrAre = "are";
        }
        else
        {
            isOrAre = "is";
        }

        return (ActionResult)new OkObjectResult($"A push occurred on GitHub repository {receivePushOutput.RepositoryName}.  The topic{s} now in the repo {isOrAre} {string.Join(", ", (receivePushOutput.TopicRaw.names))}");
    }
}
