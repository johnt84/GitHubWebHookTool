using GitHubWebHookTool.Models;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services.Interfaces
{
    public interface ITopicService
    {
        public Task<TopicRaw> GetTopics(string url);
        public Task<TopicRaw> UpdateTopics(string url, string[] names);
    }
}
