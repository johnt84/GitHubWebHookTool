using GitHubWebHookTool.Models;
using System.Threading.Tasks;

namespace GitHubWebHookTool.Services
{
    public interface ICommit
    {
        public Task<CommitRaw> GetLastCommit(string url);
    }
}
