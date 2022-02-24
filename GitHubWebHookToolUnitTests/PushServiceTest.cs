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
        //[AssemblyInitialize]
        //public static void AssemblyInit(TestContext context)
        //{
        //    // Initalization code goes here
        //}

        string repoName = string.Empty;
        string testRepoUrl = string.Empty;
        List<string> filesInCommit = new List<string>();
        List<string> fileExtensionsInCommit = new List<string>();
        List<string> topicsInCommit = new List<string>();
        List<string> existingTopicsInRepo = new List<string>(); 
        List<string> allTopicsInRepo = new List<string>();

        Mock<ICommitService> mockCommitService = new Mock<ICommitService>();
        Mock<ITopicService> mockTopicService = new Mock<ITopicService>();

        PushRaw testPushRaw = new PushRaw();

        [TestMethod]
        public async Task WhenTwoNewFileExtensionsWhichMapToTopicsAndNoExistingTopicsThen_TwoTopicsInRepo()
        {
            repoName = "TestRepo";

            testRepoUrl = $"http://www.test.com/{repoName}/";

            filesInCommit = new List<string>()
            {
                "Test.razor",
                "Program.cs",
            };

            fileExtensionsInCommit = new List<string>()
            {
                "razor",
                "cs",
            };

            topicsInCommit = new List<string>()
            {
                "blazor-server",
                "csharp",
            };

            existingTopicsInRepo = new List<string>();

            SetupTests();

            var pushService = new PushService(mockCommitService.Object, mockTopicService.Object);

            var receivePushOutput = await pushService.ReceivePushFromWebHook(testPushRaw);

            Assert.IsNotNull(receivePushOutput);
            Assert.AreEqual(repoName, receivePushOutput.RepositoryName);
            Assert.IsNotNull(receivePushOutput.TopicRaw);
            Assert.AreEqual(2, receivePushOutput.TopicsInCommit.AllTopicsInRepo.Count);
            Assert.AreEqual(allTopicsInRepo.First(), receivePushOutput.TopicsInCommit.AllTopicsInRepo.First());
            Assert.AreEqual(allTopicsInRepo.Last(), receivePushOutput.TopicsInCommit.AllTopicsInRepo.Last());
        }

        [TestMethod]
        public async Task WhenOneNewFileExtensionsWhichMapToTopicAndOneDifferentExistingTopicsThen_TwoTopicsInRepo()
        {
            repoName = "TestRepo";

            testRepoUrl = $"http://www.test.com/{repoName}/";

            filesInCommit = new List<string>()
            {
                "Program.cs",
            };

            fileExtensionsInCommit = new List<string>()
            {
                "cs",
            };

            topicsInCommit = new List<string>()
            {
                "csharp",
            };

            existingTopicsInRepo = new List<string>()
            {
                "blazor-server",
            };

            SetupTests();

            var pushService = new PushService(mockCommitService.Object, mockTopicService.Object);

            var receivePushOutput = await pushService.ReceivePushFromWebHook(testPushRaw);

            Assert.IsNotNull(receivePushOutput);
            Assert.AreEqual(repoName, receivePushOutput.RepositoryName);
            Assert.IsNotNull(receivePushOutput.TopicRaw);
            Assert.AreEqual(2, receivePushOutput.TopicsInCommit.AllTopicsInRepo.Count);
            Assert.AreEqual(allTopicsInRepo.First(), receivePushOutput.TopicsInCommit.AllTopicsInRepo.First());
            Assert.AreEqual(allTopicsInRepo.Last(), receivePushOutput.TopicsInCommit.AllTopicsInRepo.Last());
        }

        private void SetupTests()
        {
            allTopicsInRepo = topicsInCommit.Union(existingTopicsInRepo).ToList();

            var mockHttpAPIClient = new Mock<IHttpAPIClient>();
            mockHttpAPIClient.Setup(x => x.Get(testRepoUrl)).ReturnsAsync(string.Empty);

            var testLastCommit = new CommitRaw()
            {
                files = filesInCommit.Select(x => new File() { filename = x }).ToArray(),
            };

            mockCommitService = new Mock<ICommitService>();
            mockCommitService.Setup(x => x.GetLastCommit(testRepoUrl)).ReturnsAsync(testLastCommit);

            mockTopicService = new Mock<ITopicService>();

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

            testPushRaw = new PushRaw()
            {
                repository = new Repository()
                {
                    hooks_url = $"{testRepoUrl}hooks",
                    name = repoName,
                },
            };

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

            var mockPushService = new Mock<IPushService>();
            mockPushService.Setup(x => x.ReceivePushFromWebHook(testPushRaw)).ReturnsAsync(testReceivePushOutput);
        }
    }
}