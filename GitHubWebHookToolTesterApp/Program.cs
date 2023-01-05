using GitHubWebHookEngine.API;
using GitHubWebHookEngine.Services;
using GitHubWebHookShared.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

var builder = new ConfigurationBuilder()
                               .SetBasePath($"{Directory.GetCurrentDirectory()}/../../..")
                               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var config = builder.Build();

var gitHubWebHookToolInput = new GitHubWebHookToolInput()
{
    GitHubAPIUrl = config["GitHubAPIURL"],
    PrivateToken = config["PrivateToken"],
};

var httpAPIClient = new HttpAPIClient(new HttpClient(), gitHubWebHookToolInput);

var commitService = new CommitService(httpAPIClient);

var topicService = new TopicService(httpAPIClient);

var pushService = new PushService(commitService, topicService);

string pushRawJson = @"{
  ""ref"": ""refs/heads/master"",
  ""before"": ""51d047af6edd84c34a69efe132986a8f4cb786f6"",
  ""after"": ""c5ca2e9a3530824dd7b7ee6b610e7623f11532df"",
  ""repository"": {
    ""id"": 261600773,
    ""node_id"": ""MDEwOlJlcG9zaXRvcnkyNjE2MDA3NzM="",
    ""name"": ""BlazorToDoList"",
    ""full_name"": ""johnt84/BlazorToDoList"",
    ""private"": false,
    ""owner"": {
      ""name"": ""johnt84"",
      ""email"": ""33494306+johnt84@users.noreply.github.com"",
      ""login"": ""johnt84"",
      ""id"": 33494306,
      ""node_id"": ""MDQ6VXNlcjMzNDk0MzA2"",
      ""avatar_url"": ""https://avatars3.githubusercontent.com/u/33494306?v=4"",
      ""gravatar_id"": """",
      ""url"": ""https://api.github.com/users/johnt84"",
      ""html_url"": ""https://github.com/johnt84"",
      ""followers_url"": ""https://api.github.com/users/johnt84/followers"",
      ""following_url"": ""https://api.github.com/users/johnt84/following{/other_user}"",
      ""gists_url"": ""https://api.github.com/users/johnt84/gists{/gist_id}"",
      ""starred_url"": ""https://api.github.com/users/johnt84/starred{/owner}{/repo}"",
      ""subscriptions_url"": ""https://api.github.com/users/johnt84/subscriptions"",
      ""organizations_url"": ""https://api.github.com/users/johnt84/orgs"",
      ""repos_url"": ""https://api.github.com/users/johnt84/repos"",
      ""events_url"": ""https://api.github.com/users/johnt84/events{/privacy}"",
      ""received_events_url"": ""https://api.github.com/users/johnt84/received_events"",
      ""type"": ""User"",
      ""site_admin"": false
    },
    ""html_url"": ""https://github.com/johnt84/BlazorToDoList"",
    ""description"": null,
    ""fork"": true,
    ""url"": ""https://github.com/johnt84/BlazorToDoList"",
    ""forks_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/forks"",
    ""keys_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/keys{/key_id}"",
    ""collaborators_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/collaborators{/collaborator}"",
    ""teams_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/teams"",
    ""hooks_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/hooks"",
    ""issue_events_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/issues/events{/number}"",
    ""events_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/events"",
    ""assignees_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/assignees{/user}"",
    ""branches_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/branches{/branch}"",
    ""tags_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/tags"",
    ""blobs_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/git/blobs{/sha}"",
    ""git_tags_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/git/tags{/sha}"",
    ""git_refs_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/git/refs{/sha}"",
    ""trees_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/git/trees{/sha}"",
    ""statuses_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/statuses/{sha}"",
    ""languages_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/languages"",
    ""stargazers_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/stargazers"",
    ""contributors_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/contributors"",
    ""subscribers_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/subscribers"",
    ""subscription_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/subscription"",
    ""commits_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/commits{/sha}"",
    ""git_commits_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/git/commits{/sha}"",
    ""comments_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/comments{/number}"",
    ""issue_comment_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/issues/comments{/number}"",
    ""contents_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/contents/{+path}"",
    ""compare_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/compare/{base}...{head}"",
    ""merges_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/merges"",
    ""archive_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/{archive_format}{/ref}"",
    ""downloads_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/downloads"",
    ""issues_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/issues{/number}"",
    ""pulls_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/pulls{/number}"",
    ""milestones_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/milestones{/number}"",
    ""notifications_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/notifications{?since,all,participating}"",
    ""labels_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/labels{/name}"",
    ""releases_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/releases{/id}"",
    ""deployments_url"": ""https://api.github.com/repos/johnt84/BlazorToDoList/deployments"",
    ""created_at"": 1588719327,
    ""updated_at"": ""2020-05-08T18:47:59Z"",
    ""pushed_at"": 1588963959,
    ""git_url"": ""git://github.com/johnt84/BlazorToDoList.git"",
    ""ssh_url"": ""git@github.com:johnt84/BlazorToDoList.git"",
    ""clone_url"": ""https://github.com/johnt84/BlazorToDoList.git"",
    ""svn_url"": ""https://github.com/johnt84/BlazorToDoList"",
    ""homepage"": ""https://exceptionnotfound.net/using-blazor-to-build-a-client-side-single-page-app-with-net-core/"",
    ""size"": 224,
    ""stargazers_count"": 0,
    ""watchers_count"": 0,
    ""language"": ""HTML"",
    ""has_issues"": false,
    ""has_projects"": true,
    ""has_downloads"": true,
    ""has_wiki"": true,
    ""has_pages"": false,
    ""forks_count"": 0,
    ""mirror_url"": null,
    ""archived"": false,
    ""disabled"": false,
    ""open_issues_count"": 0,
    ""license"": null,
    ""forks"": 0,
    ""open_issues"": 0,
    ""watchers"": 0,
    ""default_branch"": ""master"",
    ""stargazers"": 0,
    ""master_branch"": ""master""
  },
  ""pusher"": {
    ""name"": ""johnt84"",
    ""email"": ""33494306+johnt84@users.noreply.github.com""
  },
  ""sender"": {
    ""login"": ""johnt84"",
    ""id"": 33494306,
    ""node_id"": ""MDQ6VXNlcjMzNDk0MzA2"",
    ""avatar_url"": ""https://avatars3.githubusercontent.com/u/33494306?v=4"",
    ""gravatar_id"": """",
    ""url"": ""https://api.github.com/users/johnt84"",
    ""html_url"": ""https://github.com/johnt84"",
    ""followers_url"": ""https://api.github.com/users/johnt84/followers"",
    ""following_url"": ""https://api.github.com/users/johnt84/following{/other_user}"",
    ""gists_url"": ""https://api.github.com/users/johnt84/gists{/gist_id}"",
    ""starred_url"": ""https://api.github.com/users/johnt84/starred{/owner}{/repo}"",
    ""subscriptions_url"": ""https://api.github.com/users/johnt84/subscriptions"",
    ""organizations_url"": ""https://api.github.com/users/johnt84/orgs"",
    ""repos_url"": ""https://api.github.com/users/johnt84/repos"",
    ""events_url"": ""https://api.github.com/users/johnt84/events{/privacy}"",
    ""received_events_url"": ""https://api.github.com/users/johnt84/received_events"",
    ""type"": ""User"",
    ""site_admin"": false
  },
  ""created"": false,
  ""deleted"": false,
  ""forced"": false,
  ""base_ref"": null,
  ""compare"": ""https://github.com/johnt84/BlazorToDoList/compare/51d047af6edd...c5ca2e9a3530"",
  ""commits"": [
    {
      ""id"": ""c5ca2e9a3530824dd7b7ee6b610e7623f11532df"",
      ""tree_id"": ""61cad5a208d96fa5d52f5e1b8ef15eccae10212a"",
      ""distinct"": true,
      ""message"": ""Update Program.cs"",
      ""timestamp"": ""2020-05-08T19:52:38+01:00"",
      ""url"": ""https://github.com/johnt84/BlazorToDoList/commit/c5ca2e9a3530824dd7b7ee6b610e7623f11532df"",
      ""author"": {
        ""name"": ""John Tomlinson"",
        ""email"": ""33494306+johnt84@users.noreply.github.com"",
        ""username"": ""johnt84""
      },
      ""committer"": {
        ""name"": ""GitHub"",
        ""email"": ""noreply@github.com"",
        ""username"": ""web-flow""
      },
      ""added"": [

      ],
      ""removed"": [

      ],
      ""modified"": [
        ""BlazorToDoList.App/Program.cs""
      ]
    }
  ],
  ""head_commit"": {
    ""id"": ""c5ca2e9a3530824dd7b7ee6b610e7623f11532df"",
    ""tree_id"": ""61cad5a208d96fa5d52f5e1b8ef15eccae10212a"",
    ""distinct"": true,
    ""message"": ""Update Program.cs"",
    ""timestamp"": ""2020-05-08T19:52:38+01:00"",
    ""url"": ""https://github.com/johnt84/BlazorToDoList/commit/c5ca2e9a3530824dd7b7ee6b610e7623f11532df"",
    ""author"": {
      ""name"": ""John Tomlinson"",
      ""email"": ""33494306+johnt84@users.noreply.github.com"",
      ""username"": ""johnt84""
    },
    ""committer"": {
      ""name"": ""GitHub"",
      ""email"": ""noreply@github.com"",
      ""username"": ""web-flow""
    },
    ""added"": [

    ],
    ""removed"": [

    ],
    ""modified"": [
      ""BlazorToDoList.App/Program.cs""
    ]
  }
}";

var pushRaw = JsonConvert.DeserializeObject<PushRaw>(pushRawJson);

var receivePushOutput = await pushService.ReceivePushFromWebHook(pushRaw);

string s = string.Empty;
string isOrAre = string.Empty;

if (receivePushOutput.TopicRaw?.names.Length > 1)
{
    s = "s";
    isOrAre = "are";
}
else
{
    isOrAre = "is";
}

string existingTopicsInRepoForDisplay = string.Empty;

if (receivePushOutput.TopicsInCommit?.ExistingTopicsInRepo?.Count > 0)
{
    existingTopicsInRepoForDisplay = $"\nExisting topics in repo: {string.Join(", ", receivePushOutput.TopicsInCommit.ExistingTopicsInRepo)}";
}
else
{
    existingTopicsInRepoForDisplay = $"\nThere are no existing topics in the repo";
}

Console.WriteLine($"A push occurred on the GitHub repository {receivePushOutput.RepositoryName}.  The topic{s} now in the repo {isOrAre} {string.Join(", ", ((receivePushOutput.TopicRaw.names)))}");

Console.WriteLine(existingTopicsInRepoForDisplay);

Console.WriteLine($"\nCommit Details");

Console.WriteLine($"\nFile names: {string.Join(", ", receivePushOutput.TopicsInCommit.FilesInCommit.FileNames)}");
Console.WriteLine($"File extensions: {string.Join(", ",receivePushOutput.TopicsInCommit.FilesInCommit.FileExtensions)}");
Console.WriteLine($"Topics: {string.Join(", ", receivePushOutput.TopicsInCommit.TopicsInCommit)}");

Console.WriteLine($"\nAll topics in repo: {string.Join(", ", receivePushOutput.TopicsInCommit.AllTopicsInRepo)}");
