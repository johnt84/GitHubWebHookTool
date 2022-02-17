using GitHubWebHookTool.API;
using GitHubWebHookTool.Models;
using GitHubWebHookTool.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

var builder = new ConfigurationBuilder()
                               .SetBasePath($"{Directory.GetCurrentDirectory()}/../../..")
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var config = builder.Build();

var httpClient = new HttpClient();

httpClient.BaseAddress = new Uri(config["GitHubAPIURL"]);
httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", config["PrivateToken"]);

var httpAPIClient = new HttpAPIClient(httpClient);

var commitService = new CommitService(httpAPIClient);

var topicService = new TopicService(httpAPIClient);

var pushService = new PushService(commitService, topicService);

string pushRawJson = @"";

var pushRaw = JsonConvert.DeserializeObject<PushRaw>(pushRawJson);

var topicOutput = await pushService.ReceivePushFromWebHook(pushRaw);

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

Console.WriteLine($"A push occurred on repo {topicOutput.RepositoryName}.  The topic{s} in the commit {isOrAre} {string.Join(",", ((topicOutput.TopicRaw.names)))}");
