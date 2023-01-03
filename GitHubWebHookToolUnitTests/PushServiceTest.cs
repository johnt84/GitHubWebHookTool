using GitHubWebHookEngine.API;
using GitHubWebHookEngine.Services;
using GitHubWebHookEngine.Services.Interfaces;
using GitHubWebHookShared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubWebHookToolUnitTests
{
    [TestClass]
    public class PushServiceTest
    {
        string repoName = string.Empty;
        string repoUrl = string.Empty;
        List<string> filesInCommit = new List<string>();
        List<string> fileExtensionsInCommit = new List<string>();
        List<string> topicsInCommit = new List<string>();
        List<string> existingTopicsInRepo = new List<string>(); 
        List<string> allTopicsInRepo = new List<string>();

        Mock<ICommitService> mockCommitService = new Mock<ICommitService>();
        Mock<ITopicService> mockTopicService = new Mock<ITopicService>();

        PushRaw pushRaw = new PushRaw();

        [TestMethod]
        public async Task TestWhenPushPassedInIsNull_ThenOutputIsNull()//WhenOneNewFileExtensionsWhichMapToTopicsAndOneDifferentExistingTopics_ThenTwoTopicsInRepo()
        {
            repoName = "TestRepo";

            filesInCommit = new List<string>();

            fileExtensionsInCommit = new List<string>();

            topicsInCommit = new List<string>();

            existingTopicsInRepo = new List<string>();

            SetupTests();

            pushRaw = null;

            var pushService = new PushService(mockCommitService.Object, mockTopicService.Object);

            var receivePushOutput = await pushService.ReceivePushFromWebHook(pushRaw);

            Assert.IsNull(receivePushOutput);
        }

        [TestMethod]
        public async Task TestWhenRepoNameIsNotSet_ThenNoExceptionOrError()
        {
            repoName = string.Empty;

            filesInCommit = new List<string>();

            fileExtensionsInCommit = new List<string>();

            topicsInCommit = new List<string>();

            existingTopicsInRepo = new List<string>();

            SetupTests();

            var pushService = new PushService(mockCommitService.Object, mockTopicService.Object);

            var receivePushOutput = await pushService.ReceivePushFromWebHook(pushRaw);

            Assert.IsNotNull(receivePushOutput);
            Assert.AreEqual(repoName, receivePushOutput.RepositoryName);
            Assert.IsNotNull(receivePushOutput.TopicRaw);
        }

        [TestMethod]
        public async Task TestWhenRepoUrlIsNotSet_ThenNoExceptionOrError()
        {
            repoName = "TestRepo";

            repoUrl = string.Empty;

            filesInCommit = new List<string>();

            fileExtensionsInCommit = new List<string>();

            topicsInCommit = new List<string>();

            existingTopicsInRepo = new List<string>();

            SetupTests();

            var pushService = new PushService(mockCommitService.Object, mockTopicService.Object);

            var receivePushOutput = await pushService.ReceivePushFromWebHook(pushRaw);

            Assert.IsNotNull(receivePushOutput);
            Assert.AreEqual(repoName, receivePushOutput.RepositoryName);
            Assert.IsNotNull(receivePushOutput.TopicRaw);
        }

        [TestMethod]
        public async Task WhenTwoNewFileExtensionsWhichMapToTopicsAndNoExistingTopics_ThenTwoTopicsInRepo()
        {
            repoName = "TestRepo";

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

            var receivePushOutput = await pushService.ReceivePushFromWebHook(pushRaw);

            Assert.IsNotNull(receivePushOutput);
            Assert.AreEqual(repoName, receivePushOutput.RepositoryName);
            Assert.IsNotNull(receivePushOutput.TopicRaw);
            Assert.AreEqual(2, receivePushOutput.TopicsInCommit.AllTopicsInRepo.Count);
            Assert.AreEqual(allTopicsInRepo.First(), receivePushOutput.TopicsInCommit.AllTopicsInRepo.First());
            Assert.AreEqual(allTopicsInRepo.Last(), receivePushOutput.TopicsInCommit.AllTopicsInRepo.Last());
        }

        [TestMethod]
        public async Task WhenOneNewFileExtensionsWhichMapToTopicsAndOneDifferentExistingTopics_ThenTwoTopicsInRepo()
        {
            repoName = "TestRepo";

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

            var receivePushOutput = await pushService.ReceivePushFromWebHook(pushRaw);

            Assert.IsNotNull(receivePushOutput);
            Assert.AreEqual(repoName, receivePushOutput.RepositoryName);
            Assert.IsNotNull(receivePushOutput.TopicRaw);
            Assert.AreEqual(2, receivePushOutput.TopicsInCommit.AllTopicsInRepo.Count);
            Assert.AreEqual(allTopicsInRepo.First(), receivePushOutput.TopicsInCommit.AllTopicsInRepo.First());
            Assert.AreEqual(allTopicsInRepo.Last(), receivePushOutput.TopicsInCommit.AllTopicsInRepo.Last());
        }

        [TestMethod]
        public async Task WhenOneFileExtensionsWhichMapToTopicsAndSameExistingTopic_ThenOneTopicInRepo()
        {
            repoName = "TestRepo";

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
                "csharp",
            };

            SetupTests();

            var pushService = new PushService(mockCommitService.Object, mockTopicService.Object);

            var receivePushOutput = await pushService.ReceivePushFromWebHook(pushRaw);

            Assert.IsNotNull(receivePushOutput);
            Assert.AreEqual(repoName, receivePushOutput.RepositoryName);
            Assert.IsNotNull(receivePushOutput.TopicRaw);
            Assert.AreEqual(1, receivePushOutput.TopicsInCommit.AllTopicsInRepo.Count);
            Assert.AreEqual(allTopicsInRepo.First(), receivePushOutput.TopicsInCommit.AllTopicsInRepo.First());
        }

        [TestMethod]
        public async Task WhenNoNewFileExtensionsWhichMapToTopicsAndNoExistingTopics_ThenNoTopicsInRepo()
        {
            repoName = "TestRepo";

            filesInCommit = new List<string>()
            {
                "Test.vb",
            };

            fileExtensionsInCommit = new List<string>()
            {
                "vb",
            };

            topicsInCommit = new List<string>();

            existingTopicsInRepo = new List<string>();

            SetupTests();

            var pushService = new PushService(mockCommitService.Object, mockTopicService.Object);

            var receivePushOutput = await pushService.ReceivePushFromWebHook(pushRaw);

            Assert.IsNotNull(receivePushOutput);
            Assert.AreEqual(repoName, receivePushOutput.RepositoryName);
            Assert.IsNotNull(receivePushOutput.TopicRaw);
            Assert.AreEqual(0, receivePushOutput.TopicsInCommit.AllTopicsInRepo.Count);
        }

        [TestMethod]
        public async Task WhenNoNewFileExtensionsWhichMapToTopicsAndOneExistingTopic_ThenOneTopicInRepo()
        {
            repoName = "TestRepo";

            filesInCommit = new List<string>()
            {
                "Test.vb",
            };

            fileExtensionsInCommit = new List<string>()
            {
                "vb",
            };

            topicsInCommit = new List<string>();

            existingTopicsInRepo = new List<string>(){
                "blazor-server",
            };

            SetupTests();

            var pushService = new PushService(mockCommitService.Object, mockTopicService.Object);

            var receivePushOutput = await pushService.ReceivePushFromWebHook(pushRaw);

            Assert.IsNotNull(receivePushOutput);
            Assert.AreEqual(repoName, receivePushOutput.RepositoryName);
            Assert.IsNotNull(receivePushOutput.TopicRaw);
            Assert.AreEqual(1, receivePushOutput.TopicsInCommit.AllTopicsInRepo.Count);
            Assert.AreEqual(allTopicsInRepo.First(), receivePushOutput.TopicsInCommit.AllTopicsInRepo.First());
        }

        private void SetupTests()
        {
            repoUrl = $"http://www.test.com/{repoName}/";

            allTopicsInRepo = topicsInCommit
                                .Union(existingTopicsInRepo)
                                .OrderBy(x => x)
                                .ToList();

            var mockHttpAPIClient = new Mock<IHttpAPIClient>();
            mockHttpAPIClient.Setup(x => x.Get(repoUrl)).ReturnsAsync(string.Empty);

            var testLastCommit = new CommitRaw()
            {
                files = filesInCommit.Select(x => new File() { filename = x }).ToArray(),
            };

            mockCommitService = new Mock<ICommitService>();
            mockCommitService.Setup(x => x.GetLastCommit(repoUrl)).ReturnsAsync(testLastCommit);

            mockTopicService = new Mock<ITopicService>();

            var topicsInCommitRaw = new TopicRaw()
            {
                names = existingTopicsInRepo.ToArray(),
            };

            mockTopicService.Setup(x => x.GetTopics(repoUrl)).ReturnsAsync(topicsInCommitRaw);

            var allTopicsInRepoArray = allTopicsInRepo.ToArray();

            var updateTopicRawOutput = new TopicRaw()
            {
                names = allTopicsInRepoArray,
            };

            mockTopicService.Setup(x => x.UpdateTopics(repoUrl, allTopicsInRepoArray)).ReturnsAsync(updateTopicRawOutput);

            pushRaw = new PushRaw()
            {
                repository = new Repository()
                {
                    hooks_url = $"{repoUrl}hooks",
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
            mockPushService.Setup(x => x.ReceivePushFromWebHook(pushRaw)).ReturnsAsync(testReceivePushOutput);
        }
    }
}