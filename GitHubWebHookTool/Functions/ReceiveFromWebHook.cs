using GitHubWebHookTool.API;
using GitHubWebHookTool.Models;
using GitHubWebHookTool.Services;
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
        private readonly IPush _push;
        private readonly HttpAPIClient _httpAPIClient;

        public ReceiveFromWebHook(IPush push, HttpAPIClient httpAPIClient)
        {
            _push = push;
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

            string repo = pushRaw?.repository?.name;

            pushRaw = await _push.ReceivePushFromWebHook(pushRaw);

            return (ActionResult)new OkObjectResult($"We got data on {repo}.");
        }
    }
}
