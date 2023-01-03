using GitHubWebHookEngine.Services.Interfaces;
using GitHubWebHookEngine.API;
using GitHubWebHookShared.Models;
using Newtonsoft.Json;

namespace GitHubWebHookEngine.Services
{
    public class TopicService : ITopicService
    {
        private HttpAPIClient _httpAPIClient;

        public TopicService(HttpAPIClient httpAPIClient)
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
