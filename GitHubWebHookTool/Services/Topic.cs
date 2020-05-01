using GitHubWebHookTool.API;
using GitHubWebHookTool.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services
{
    public class Topic : ITopic
    {
        private HttpAPIClient _httpAPIClient;

        public Topic(HttpAPIClient httpAPIClient)
        {
            _httpAPIClient = httpAPIClient;
        }

        public async Task<TopicRaw> GetTopics(string url)
        {
            string json = await _httpAPIClient.Get($"{ url }topics");

            return JsonConvert.DeserializeObject<TopicRaw>(json);
        }

        public async Task<TopicRaw> UpdateTopics(string url, string[] names)
        {
            var result = await _httpAPIClient.Put($"{ url }topics", names);

            return JsonConvert.DeserializeObject<TopicRaw>(result);
        }
    }
}
