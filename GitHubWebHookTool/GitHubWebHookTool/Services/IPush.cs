using GitHubWebHookTool.Models;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services
{
    public interface IPush
    {
        Task<PushRaw> ReceivePushFromWebHook(PushRaw pushRaw);
    }
}
