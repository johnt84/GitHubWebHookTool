using GitHubWebHookTool.API;
using GitHubWebHookTool.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services
{
    public class Commit : ICommit
    {
        private HttpAPIClient _httpAPIClient;

        public Commit(HttpAPIClient httpAPIClient)
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
