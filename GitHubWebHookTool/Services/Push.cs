using GitHubWebHookTool.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services
{
    public class Push : IPush
    {
        private ICommit _commit;
        private ITopic _topic;

        public Push(ICommit commit, ITopic topic)
        {
            _commit = commit;
            _topic = topic;
        }

        public async Task<PushRaw> ReceivePushFromWebHook(PushRaw pushRaw, Dictionary<string, string> fileExtensionTopicMappings)
        {
            if (pushRaw == null)
            {
                return pushRaw;
            }

            string repositoryURL = pushRaw.repository.hooks_url
                                    .Substring(0, pushRaw.repository.hooks_url
                                    .IndexOf("hooks"));

            var topicsInCommit = await GetTopicsInCommit(repositoryURL, fileExtensionTopicMappings);

            var postedNewTopicsInCommit = await _topic.UpdateTopics(repositoryURL, topicsInCommit.ToArray());

            return pushRaw;
        }



        private List<string> GetFileExtensionsInCommmit(CommitRaw commit)
        {
            var fileExtensionsInCommit = commit.files
                                    .Select(x => Path.GetExtension(x.filename).Replace(".", ""))
                                    .ToList();

            return fileExtensionsInCommit;
        }

        private async Task<List<string>> GetTopicsInCommit(string repositoryURL, Dictionary<string, string> fileExtensionTopicMappings)
        {
            var lastCommit = await _commit.GetLastCommit(repositoryURL);

            var fileExtensionsInCommmit = GetFileExtensionsInCommmit(lastCommit);

            var topicsInCommit = fileExtensionTopicMappings
                                        .Where(x => fileExtensionsInCommmit.Contains(x.Key))
                                        .Select(x => x.Value)
                                        .ToList();

            var existingTopicsInRepo = await _topic.GetTopics(repositoryURL);

            return existingTopicsInRepo.names.ToList().Union(topicsInCommit).ToList();
        }
    }
}
