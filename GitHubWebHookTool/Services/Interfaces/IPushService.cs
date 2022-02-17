using GitHubWebHookTool.Models;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services.Interfaces
{
    public interface IPushService
    {
        Task<TopicOutput> ReceivePushFromWebHook(PushRaw pushRaw);
    }
}
