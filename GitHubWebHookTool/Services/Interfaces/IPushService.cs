using GitHubWebHookTool.Models;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services.Interfaces
{
    public interface IPushService
    {
        Task<ReceivePushOutput> ReceivePushFromWebHook(PushRaw pushRaw);
    }
}
