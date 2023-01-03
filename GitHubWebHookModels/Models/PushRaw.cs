using System;

namespace GitHubWebHookShared.Models
{
    public class PushRaw
    {
        public Hook hook { get; set; }
        public Repository repository { get; set; }
    }

    public class Hook
    {
        public string[] events { get; set; }
        public string url { get; set; }
    }

    public class Repository
    {
        public string name { get; set; }
        public string hooks_url { get; set; }
    }
}
