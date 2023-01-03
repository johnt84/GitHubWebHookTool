using GitHubWebHookShared.Models;

namespace GitHubWebHookEngine.Services.Interfaces
{
    public interface IPushService
    {
        Task<ReceivePushOutput> ReceivePushFromWebHook(PushRaw pushRaw);
    }
}
