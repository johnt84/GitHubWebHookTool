using GitHubWebHookEngine.API;
using GitHubWebHookEngine.Services.Interfaces;
using GitHubWebHookShared.Models;
using Newtonsoft.Json;

namespace GitHubWebHookEngine.Services
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
