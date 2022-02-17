using GitHubWebHookTool.API;
using GitHubWebHookTool.Models;
using GitHubWebHookTool.Services.Interfaces;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services
{
    public class CommitService : ICommitService
    {
        private HttpAPIClient _httpAPIClient;

        public CommitService(HttpAPIClient httpAPIClient)
        {
            _httpAPIClient = httpAPIClient;
        }

        public async Task<CommitRaw> GetLastCommit(string url)
        {
            string json = await _httpAPIClient.Get($"{ url }commits/master");

            return JsonConvert.DeserializeObject<CommitRaw>(json);
        }
    }
}
