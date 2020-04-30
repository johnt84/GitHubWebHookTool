# GitHubWebHookTool
Simple Git Hub Web Hook Azure Function

* GitHub Webhook which listens for new commits
* It maintains the list of topics on a GitHub repository by checking the last commit and obtaining the file extensions and then updating the topics based on the last commie.  For example if the last commit contains a file with a .cs extension then the Azure Function will ensure that csharp is a topic on the github repository
* The current file extension to topic mappings (shown below) is stored in appsettings.json
* Developed as an Azure Function 1.0/.Net Core 3.1

| File Extension  | Topic |
| ------------- | ------------- |
| aspx  | web-forms |
| cs  | csharp  |
| js  | javascript  |
| razor  | blazor-server  |
| sql  | tsql  |
