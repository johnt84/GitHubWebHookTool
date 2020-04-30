using GitHubWebHookTool.API;
using GitHubWebHookTool.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services
{
    public class Push : IPush
    {
        HttpAPIClient _httpAPIClient;
        ConfigurationItems _configurationItems;
        ICommit _commit;
        ITopic _topic;

        public Push(HttpAPIClient httpAPIClient, IOptions<ConfigurationItems> configurationItems, ICommit commit, ITopic topic)
        {
            _httpAPIClient = httpAPIClient;
            _configurationItems = configurationItems.Value;
            _commit = commit;
            _topic = topic;
        }

        public async Task<PushRaw> ReceivePushFromWebHook(PushRaw pushRaw)
        {
            if (pushRaw == null)
            {
                return pushRaw;
            }

            string repositoryURL = pushRaw.hook.url.Substring(0, pushRaw.hook.url.IndexOf("hooks"));

            var topicsInCommit = await GetTopicsInCommit(repositoryURL);

            var postedNewTopicsInCommit = await _topic.UpdateTopics(repositoryURL, topicsInCommit.ToArray());

            return pushRaw;
        }



        private List<string> GetFileExtensionsInCommmit(CommitRaw commit)
        {
            var fileExtensionsInCommit = commit.files
                                    .Select(x => x.filename
                                                    .Substring(x.filename.IndexOf(".") + 1, x.filename.Length - (x.filename.IndexOf(".") + 1)))
                                    .ToList();

            return fileExtensionsInCommit;
        }

        private async Task<List<string>> GetTopicsInCommit(string repositoryURL)
        {
            var lastCommit = await _commit.GetLastCommit(repositoryURL);

            var fileExtensionsInCommmit = GetFileExtensionsInCommmit(lastCommit);

            //fileExtensionsInCommmit = new List<string>() { "cs" };

            var topicsInCommit = _configurationItems.FileExtensionTopicMappings
                                        .Where(x => fileExtensionsInCommmit.Contains(x.Key))
                                        .Select(x => x.Value)
                                        .ToList();

            var existingTopicsInRepo = await _topic.GetTopics(repositoryURL);

            return existingTopicsInRepo.names.ToList().Union(topicsInCommit).ToList();
        }
    }
}
