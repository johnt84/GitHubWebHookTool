using GitHubWebHookEngine.Services.Interfaces;
using GitHubWebHookShared.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GitHubWebHookAWSServerlessTool.Controllers
{
    [Route("api/[controller]")]
    public class ReceiveFromWebHookController : ControllerBase
    {
        private readonly ILogger<ReceiveFromWebHookController> _logger;
        private readonly IPushService _pushService;

        public ReceiveFromWebHookController(ILogger<ReceiveFromWebHookController> logger, IPushService pushService)
        {
            _logger = logger;
            _pushService = pushService;
        }

        [HttpPost]
        public async Task<ActionResult> ReceivePush([FromBody] PushRaw pushRaw)
        {
            _logger.LogInformation($"ReceiveFromWebHook received a push request pushRaw: {JsonConvert.SerializeObject(pushRaw)}");

            if (pushRaw == null)
            {
                return BadRequest("Invalid Push Raw Input");
            }

            var receivePushOutput = await _pushService.ReceivePushFromWebHook(pushRaw);

            _logger.LogInformation($"\nReceivePushOutput returned from the PushService: {JsonConvert.SerializeObject(receivePushOutput)}");

            if (receivePushOutput == null)
            {
                return BadRequest("Invalid Push Raw Input");
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

            return Ok($"A push occurred on GitHub repository {receivePushOutput.RepositoryName}.  The topic{s} now in the repo {isOrAre} {string.Join(", ", (receivePushOutput.TopicRaw?.names))}");
        }
    }
}
