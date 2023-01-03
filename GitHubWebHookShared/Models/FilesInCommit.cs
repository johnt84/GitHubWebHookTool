using System.Collections.Generic;

namespace GitHubWebHookShared.Models
{
    public class FilesInCommit
    {
        public List<string> FileNames { get; set; }
        public List<string> FileExtensions { get; set;}
    }
}
