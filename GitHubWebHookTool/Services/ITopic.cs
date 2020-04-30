using GitHubWebHookTool.Models;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services
{
    public interface ITopic
    {
        public Task<TopicRaw> GetTopics(string url);
        public Task<TopicRaw> UpdateTopics(string url, string[] names);
    }
}
