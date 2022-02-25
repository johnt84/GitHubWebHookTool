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
        private ICommitService _commitService;
        private ITopicService _topicService;
        private Dictionary<string, string> _fileExtensionTopicMappings;

        public PushService(ICommitService commitService, ITopicService topicService)
        {
            _commitService = commitService;
            _topicService = topicService;
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

            var topicRaw = await _topicService.UpdateTopics(repositoryURL, topicsInCommit.AllTopicsInRepo.ToArray());

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
            var lastCommit = await _commitService.GetLastCommit(repositoryURL);

            var filesInCommmit = GetFileExtensionsInCommmit(lastCommit);

            var topicsInCommit = _fileExtensionTopicMappings
                                        .Where(x => filesInCommmit.FileExtensions.Contains(x.Key))
                                        .Select(x => x.Value)
                                        .ToList();

            var existingTopicsInRepo = await _topicService.GetTopics(repositoryURL);

            var allTopicsInRepo = existingTopicsInRepo
                                    .names.ToList()
                                    .Union(topicsInCommit)
                                    .OrderBy(x => x)
                                    .ToList();

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
