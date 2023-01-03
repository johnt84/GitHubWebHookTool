namespace GitHubWebHookShared.Models
{
    public class CommitRaw
    {
        public File[] files { get; set; }
    }

    public class File
    {
        public string filename { get; set; }
    }
}
