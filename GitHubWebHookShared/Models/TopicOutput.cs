namespace GitHubWebHookShared.Models
{
    public class TopicOutput
    {
        public FilesInCommit FilesInCommit { get; set; }
        public List<string> TopicsInCommit { get; set; }
        public List<string> ExistingTopicsInRepo { get; set; }
        public List<string> AllTopicsInRepo { get; set; }
    }
}
