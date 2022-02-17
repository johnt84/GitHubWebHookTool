using GitHubWebHookTool.Models;
using GitHubWebHookTool.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services
{
    public class PushService : IPushService
    {
        private ICommitService _commit;
        private ITopicService _topic;
        private Dictionary<string, string> _fileExtensionTopicMappings;

        public PushService(ICommitService commit, ITopicService topic)
        {
            _commit = commit;
            _topic = topic;
            _fileExtensionTopicMappings = new Dictionary<string, string>()
            {
                { "aspx", "web-forms" },
                { "cs", "csharp" },
                { "js", "javascript" },
                { "razor", "blazor-server" },
                { "sql", "tsqls" }
            };
        }

        public async Task<TopicOutput> ReceivePushFromWebHook(PushRaw pushRaw)
        {
            if (pushRaw == null)
            {
                return null;
            }

            string repositoryURL = pushRaw.repository.hooks_url
                                    .Substring(0, pushRaw.repository.hooks_url
                                    .IndexOf("hooks"));

            var topicsInCommit = await GetTopicsInCommit(repositoryURL);

            var topicRaw = await _topic.UpdateTopics(repositoryURL, topicsInCommit.ToArray());

            return new TopicOutput()
            {
                RepositoryName = pushRaw.repository?.name ?? string.Empty,
                TopicRaw = topicRaw,
            };
        }



        private List<string> GetFileExtensionsInCommmit(CommitRaw commit)
        {
            var fileExtensionsInCommit = commit.files
                                    .Select(x => Path.GetExtension(x.filename).Replace(".", ""))
                                    .ToList();

            return fileExtensionsInCommit;
        }

        private async Task<List<string>> GetTopicsInCommit(string repositoryURL)
        {
            var lastCommit = await _commit.GetLastCommit(repositoryURL);

            var fileExtensionsInCommmit = GetFileExtensionsInCommmit(lastCommit);

            var topicsInCommit = _fileExtensionTopicMappings
                                        .Where(x => fileExtensionsInCommmit.Contains(x.Key))
                                        .Select(x => x.Value)
                                        .ToList();

            var existingTopicsInRepo = await _topic.GetTopics(repositoryURL);

            return existingTopicsInRepo.names.ToList().Union(topicsInCommit).ToList();
        }
    }
}
