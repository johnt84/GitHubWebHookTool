namespace GitHubWebHookTool.Models
{
    public class ReceivePushOutput
    {
        public string RepositoryName { get; set; }
        public TopicOutput TopicsInCommit { get; set; }
        public TopicRaw TopicRaw { get; set; }

    }
}
