using GitHubWebHookShared.Models;

namespace GitHubWebHookEngine.Services.Interfaces
{
    public interface ICommitService
    {
        public Task<CommitRaw> GetLastCommit(string url);
    }
}
