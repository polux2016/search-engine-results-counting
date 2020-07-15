# Search engine results counting

## Requirements
- The application will check the results count for different engines (MSN and Google) for one or few words.
- The words you can add in parameters 
- The report will be in console

## Run Tests
```
dotnet test 
```
## Run Application. 
Got to "SearchEngineResultsCounting" and run next
```
dotnet run "phrase one" "phrase two" [any other phrases] 
```

# Steps to init solution and project
```PowerShell
dotnet new sln --name SearchEngineResultsCounting
dotnet new console --name SearchEngineResultsCounting
dotnet sln add SearchEngineResultsCounting/SearchEngineResultsCounting.csproj
dotnet sln add SearchEngineResultsCounting.Tests/SearchEngineResultsCounting.Tests.csproj
```

# Packages 
```PowerShell
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.FileExtensions
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging
dotnet add package Microsoft.Extensions.Logging.Console
dotnet add package Microsoft.Extensions.Http
dotnet add package System.Json

dotnet add package Moq 
dotnet add package xunit 
dotnet add package xunit.runner.visualstudio 
dotnet add package coverlet.collector
dotnet add package Microsoft.Extensions.Logging
```

# Google Search 
## KEY 
AIzaSyCp3LWWCVszsF_ES1JXA5OsRA27If3CGHU
## Example 
```http
GET https://www.googleapis.com/customsearch/v1?key=AIzaSyCp3LWWCVszsF_ES1JXA5OsRA27If3CGHU&cx=017576662512468239146:omuauf_lfve&q=[text to find]
```

# Msn Search 
## KEY 
7947afa74a6647eaa3838c2cfc394ebf
## Example

Header

Key `Ocp-Apim-Subscription-Key` 

Value `7947afa74a6647eaa3838c2cfc394ebf`

```http
GET https://api.cognitive.microsoft.com/bing/v7.0/news/search?q=test
```

# Plans 
 - Add reading configuration form Cloud 
 - Check how to work with configuration like an object, not like a dictionary 