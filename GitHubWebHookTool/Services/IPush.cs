using GitHubWebHookTool.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services
{
    public interface IPush
    {
        Task<PushRaw> ReceivePushFromWebHook(PushRaw pushRaw, Dictionary<string, string> fileExtensionTopicMappings);
    }
}
