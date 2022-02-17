using GitHubWebHookTool.Models;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services.Interfaces
{
    public interface ICommitService
    {
        public Task<CommitRaw> GetLastCommit(string url);
    }
}
