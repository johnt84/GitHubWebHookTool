# GitHub Webhook Azure Function
Simple GitHub Webhook Azure Function

* GitHub Webhook which listens for new commits (git push)
* It maintains the list of topics on a GitHub repository by obtaining the file extensions in the last commit and then updating the topics in the GitHub repository based on the last commit.  For example if the last commit contains a file with a .cs extension then the Azure Function will ensure that csharp is added as a topic on the GitHub repository using the GitHub api
* The current file extension to topic mappings (shown below) is stored in appsettings.json
* Developed with Azure Functions 4.0/.Net 6
* Contains a unit test app which uses MS Test .Net 6 and utilises the Moq 4.16 library to unit test the push service
* There is also AWS Lambda Function versions of the GitHub WebHook which are:-
   + AWS Lamda Function
   + AWS Serverless API Function
* The GitHub WebHook Azure Function and AWS Lambda Functions utilise: 
    + Shared GitHubWebHookEngine Class Library (.Net 6)
    + Shared Models Class Library (.Net 6)

| File Extension  | Topic |
| ------------- | ------------- |
| aspx  | web-forms |
| cs  | csharp  |
| js  | javascript  |
| razor  | blazor-server  |
| sql  | tsql  |

## Example update

### GitHub Repo with one topics before update 

![image](https://user-images.githubusercontent.com/33494306/222495241-7d54c285-ce18-49f2-a7f9-21cbe2456610.png)

###Last commit with one mapped Topic of C#

Two file extensions of .cs and .json in last commit but only .cs is mapped to csharp topic.

![image](https://user-images.githubusercontent.com/33494306/222494086-9e453901-33dd-41cb-9c0e-c65b4acc220b.png)

###Postman Request and Response showing updated list of topics on GitHub repo

GitHubWebHookTool maintains the list of current topics and adds any extra topics not already in the current list of topics in the repo.  Therefore here there was already an existing topic of football-data and the csharp topic is added to the list.

![image](https://user-images.githubusercontent.com/33494306/222493852-f4d3e727-48e5-4df9-9eaa-59ced0112358.png)

###Repo with C# topics after update

![image](https://user-images.githubusercontent.com/33494306/222495109-ac675de6-96ca-4610-9d74-597739aa5635.png)

