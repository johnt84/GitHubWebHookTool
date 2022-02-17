using GitHubWebHookTool.API;
using GitHubWebHookTool.Models;
using GitHubWebHookTool.Services;
using GitHubWebHookTool.Services.Interfaces;
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
        private readonly HttpAPIClient _httpAPIClient;

        public ReceiveFromWebHook(IPushService pushService, HttpAPIClient httpAPIClient)
        {
            _pushService = pushService;
            _httpAPIClient = httpAPIClient;
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

            var topicOutput = await _pushService.ReceivePushFromWebHook(pushRaw);

            string s = string.Empty;
            string isOrAre = string.Empty;

            if (topicOutput.TopicRaw?.names.Length > 1)
            {
                s = "s";
                isOrAre = "are";
            }
            else
            {
                isOrAre = "is";
            }

            return (ActionResult)new OkObjectResult($"A push occurred on repo {topicOutput.RepositoryName}.  The topic{s} in the commit {isOrAre} {string.Join(",", (topicOutput.TopicRaw.names))}");
        }
    }
}
