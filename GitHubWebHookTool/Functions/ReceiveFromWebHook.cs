using GitHubWebHookEngine.API;
using GitHubWebHookEngine.Services.Interfaces;
using GitHubWebHookShared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace GitHubWebHookTool
{
    public class ReceiveFromWebHook
    {
        private readonly IPushService _pushService;

        public ReceiveFromWebHook(IPushService pushService)
        {
            _pushService = pushService;
        }

        [FunctionName("ReceiveFromWebHook")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger log)
        {
            log.LogInformation("HTTP trigger function processed a GitHub webhook request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var pushRaw = JsonConvert.DeserializeObject<PushRaw>(requestBody);

            var receivePushOutput = await _pushService.ReceivePushFromWebHook(pushRaw);

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
}
