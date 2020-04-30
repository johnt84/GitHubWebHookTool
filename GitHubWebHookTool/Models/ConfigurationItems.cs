using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubWebHookTool.Models
{
    public class ConfigurationItems
    {
        public string GitHubAPIUrl { get; set; }
        public string PrivateToken { get; set; }
        public Dictionary<string, string> FileExtensionTopicMappings { get; set; }
    }
}
