# GitHub Webhook Azure Function
Simple GitHub Webhook Azure Function

* GitHub Webhook which listens for new commits (git push)
* It maintains the list of topics on a GitHub repository by obtaining the file extensions in the last commit and then updating the topics in the GitHub repository based on the last commit.  For example if the last commit contains a file with a .cs extension then the Azure Function will ensure that csharp is added as a topic on the github repository using the GitHub api
* The current file extension to topic mappings (shown below) is stored in appsettings.json
* Developed with Azure Functions 1.0/.Net Core 3.1

| File Extension  | Topic |
| ------------- | ------------- |
| aspx  | web-forms |
| cs  | csharp  |
| js  | javascript  |
| razor  | blazor-server  |
| sql  | tsql  |
