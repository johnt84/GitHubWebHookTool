using GitHubWebHookTool.API;
using GitHubWebHookTool.Models;
using GitHubWebHookTool.Services;
using GitHubWebHookTool.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GitHubWebHookToolUnitTests
{
    [TestClass]
    public class PushServiceTest
    {
        [TestMethod]
        public async Task ReceivePushFromWebHookTest()
        {
            string repoName = "TestRepo";

            string testRepoUrl = $"http://www.test.com/{repoName}/";

            var filesInCommit = new List<string>()
            {
                "Test.razor",
                "Program.cs",
            };

            var fileExtensionsInCommit = new List<string>()
            {
                "razor",
                "cs",
            };

            var topicsInCommit = new List<string>()
            {
                "blazor-server",
                "csharp",
            };

            var existingTopicsInRepo = new List<string>();
            //{
            //    "blazor-server",
            //};

            var allTopicsInRepo = existingTopicsInRepo.Union(topicsInCommit).ToList();

            var testReceivePushOutput = new ReceivePushOutput()
            {
                RepositoryName = repoName,
                TopicsInCommit = new TopicOutput()
                {
                    FilesInCommit = new FilesInCommit()
                    {
                        FileNames = filesInCommit,
                        FileExtensions = fileExtensionsInCommit,
                    },
                    TopicsInCommit = topicsInCommit,
                    ExistingTopicsInRepo = existingTopicsInRepo,
                    AllTopicsInRepo = allTopicsInRepo,
                },
                TopicRaw = new TopicRaw()
                {
                    names = topicsInCommit.ToArray(),
                }
            };

            var mockHttpAPIClient = new Mock<IHttpAPIClient>();
            mockHttpAPIClient.Setup(x => x.Get(testRepoUrl)).ReturnsAsync(string.Empty);

            var testLastCommit = new CommitRaw()
            {
                files = filesInCommit.Select(x => new File() { filename = x }).ToArray(),
            };

            var mockCommitService = new Mock<ICommitService>();
            mockCommitService.Setup(x => x.GetLastCommit(testRepoUrl)).ReturnsAsync(testLastCommit);

            var mockTopicService = new Mock<ITopicService>();

            var topicsInCommitRaw = new TopicRaw()
            {
                names = topicsInCommit.ToArray(),
            };

            mockTopicService.Setup(x => x.GetTopics(testRepoUrl)).ReturnsAsync(topicsInCommitRaw);

            var allTopicsInRepoArray = allTopicsInRepo.ToArray();

            var updateTopicRawOutput = new TopicRaw()
            {
                names = allTopicsInRepoArray,
            };

            mockTopicService.Setup(x => x.UpdateTopics(testRepoUrl, allTopicsInRepoArray)).ReturnsAsync(updateTopicRawOutput);

            var testPushRaw = new PushRaw()
            {
                repository = new Repository()
                {
                    hooks_url = $"{testRepoUrl}hooks",
                    name = repoName,
                },
            };

            var mockPushService = new Mock<IPushService>();
            mockPushService.Setup(x => x.ReceivePushFromWebHook(testPushRaw)).ReturnsAsync(testReceivePushOutput);

            var pushService = new PushService(mockCommitService.Object, mockTopicService.Object);

            var receivePushOutput = await pushService.ReceivePushFromWebHook(testPushRaw);
        }
    }
}