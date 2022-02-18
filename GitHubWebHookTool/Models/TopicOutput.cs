using System.Collections.Generic;

namespace GitHubWebHookTool.Models
{
    public class TopicOutput
    {
        public FilesInCommit FilesInCommit { get; set; }
        public List<string> TopicsInCommit { get; set; }
        public List<string> ExistingTopicsInRepo { get; set; }
        public List<string> AllTopicsInRepo { get; set; }
    }
}
