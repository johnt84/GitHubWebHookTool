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
            string repoName = "BlazorToDoList";

            string testRepoUrl = $"https://api.github.com/repos/johnt84/{repoName}/";

            var filesInCommit = new List<string>()
            {
                "BlazorToDoList.App",
                "Program.cs",
            };

            var fileExtensionsInCommit = new List<string>()
            {
                "cs",
            };

            var topicsInCommit = new List<string>()
            {
                "csharp",
            };

            var existingTopicsInRepo = new List<string>()
            {
                "csharp",
            };

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

            var updateTopicRawOutput = new TopicRaw()
            {
                names = allTopicsInRepo.ToArray(),
            };

            mockTopicService.Setup(x => x.UpdateTopics(testRepoUrl, allTopicsInRepo.ToArray())).ReturnsAsync(updateTopicRawOutput);

            var testPushRaw = new PushRaw()
            {
                repository = new Repository()
                {
                    hooks_url = $"{testRepoUrl}hooks",
                    name = repoName,
                },
            };

            string testReceivePushOutputJson = @"{
                                       ""RepositoryName"":""BlazorToDoList"",
                                       ""TopicsInCommit"":{
                                                    ""FilesInCommit"":{
                                                        ""FileNames"":[
                                                           ""BlazorToDoList.App / Program.cs""
                                                       ],
                                             ""FileExtensions"":[
                                                ""cs""
                                                       ]
                                          },
                                          ""TopicsInCommit"":[
                                             ""csharp""
                                          ],
                                          ""ExistingTopicsInRepo"":[
                                             ""csharp""
                                          ],
                                          ""AllTopicsInRepo"":[
                                             ""csharp""
                                          ]
                                       },
                                       ""TopicRaw"":{
                                                    ""names"":[
                                                       ""csharp""
                                          ]
                                       }
                                            }";

            var testReceivePushOutputOld = JsonConvert.DeserializeObject<ReceivePushOutput>(testReceivePushOutputJson);

            var mockPushService = new Mock<IPushService>();
            mockPushService.Setup(x => x.ReceivePushFromWebHook(testPushRaw)).ReturnsAsync(testReceivePushOutputOld); //testReceivePushOutput

            var pushService = new PushService(mockCommitService.Object, mockTopicService.Object);

            var receivePushOutput = await pushService.ReceivePushFromWebHook(testPushRaw);
        }
    }
}