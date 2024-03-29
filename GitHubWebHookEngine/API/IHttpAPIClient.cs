﻿namespace GitHubWebHookEngine.API
{
    public interface IHttpAPIClient
    {
        public Task<string> Get(string url);
        public Task<string> Put(string url, string[] messages);
    }
}
