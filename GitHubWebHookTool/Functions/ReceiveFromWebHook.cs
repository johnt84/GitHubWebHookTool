using GitHubWebHookTool.API;
using GitHubWebHookTool.Models;
using GitHubWebHookTool.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GitHubWebHookTool
{
    public class ReceiveFromWebHook
    {
        private readonly ConfigurationItems _configurationItems;
        private readonly IPush _push;
        private readonly HttpAPIClient _httpAPIClient;

        public ReceiveFromWebHook(IOptions<ConfigurationItems> configurationItems, IPush push, HttpAPIClient httpAPIClient)
        {
            _configurationItems = configurationItems.Value;
            _push = push;
            _httpAPIClient = httpAPIClient;
        }

        [FunctionName("ReceiveFromWebHook")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger log)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var test3 = new Dictionary<string, string>();
            config.Bind("FileExtensionTopicMappings", test3);

            //log.LogInformation($"FileExtensionTopicMappings: {FileExtensionTopicMappings}");

            log.LogInformation("HTTP trigger function processed a GitHub webhook request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var pushRaw = JsonConvert.DeserializeObject<PushRaw>(requestBody);

            string repo = pushRaw?.repository?.name;

            pushRaw = await _push.ReceivePushFromWebHook(pushRaw);

            // respond
            return (ActionResult)new OkObjectResult($"We got data on {repo}.");
        }
    }
}
