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

        public async Task<ReceivePushOutput> ReceivePushFromWebHook(PushRaw pushRaw)
        {
            if (pushRaw == null)
            {
                return null;
            }

            string repositoryURL = pushRaw.repository.hooks_url
                                    .Substring(0, pushRaw.repository.hooks_url
                                    .IndexOf("hooks"));

            var topicsInCommit = await GetTopicsInCommit(repositoryURL);

            var topicRaw = await _topic.UpdateTopics(repositoryURL, topicsInCommit.AllTopicsInRepo.ToArray());

            return new ReceivePushOutput()
            {
                RepositoryName = pushRaw.repository?.name ?? string.Empty,
                TopicsInCommit = topicsInCommit,
                TopicRaw = topicRaw,
            };
        }



        private FilesInCommit GetFileExtensionsInCommmit(CommitRaw commit)
        {
            var filesNamesInCommit = commit.files.Select(x => x.filename).ToList();

            var fileExtensionsInCommit = commit.files
                                            .Select(x => Path.GetExtension(x.filename).Replace(".", ""))
                                            .ToList();

            return new FilesInCommit()
            {
                FileNames = filesNamesInCommit,
                FileExtensions = fileExtensionsInCommit,
            };
        }

        private async Task<TopicOutput> GetTopicsInCommit(string repositoryURL)
        {
            var lastCommit = await _commit.GetLastCommit(repositoryURL);

            var filesInCommmit = GetFileExtensionsInCommmit(lastCommit);

            var topicsInCommit = _fileExtensionTopicMappings
                                        .Where(x => filesInCommmit.FileExtensions.Contains(x.Key))
                                        .Select(x => x.Value)
                                        .ToList();

            var existingTopicsInRepo = await _topic.GetTopics(repositoryURL);

            var allTopicsInRepo = existingTopicsInRepo.names.ToList().Union(topicsInCommit).ToList();

            return new TopicOutput()
            {
                FilesInCommit = filesInCommmit,
                TopicsInCommit = topicsInCommit,
                ExistingTopicsInRepo = existingTopicsInRepo.names.ToList(),
                AllTopicsInRepo = allTopicsInRepo,
            };
        }
    }
}
