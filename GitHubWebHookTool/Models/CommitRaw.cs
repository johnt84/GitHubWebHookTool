using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubWebHookTool.Models
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
