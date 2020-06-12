# search-engine-results-counting
Search engine results counting
## Run Tests
dotnet test
## Run Application
dotnet run [the list of words to search]

# Steps to init solution and project
dotnet new sln --name SearchEngineResultsCounting
dotnet new console --name SearchEngineResultsCounting
dotnet sln add SearchEngineResultsCounting/SearchEngineResultsCounting.csproj
dotnet sln add SearchEngineResultsCounting.Tests/SearchEngineResultsCounting.Tests.csproj

# Packages 
dotnet add package Microsoft.Extensions.DependencyInjection
c
dotnet add package Microsoft.Extensions.Logging.Console
dotnet add package Microsoft.Extensions.Http
dotnet add package Moq 

# Google Search 
## KEY 
AIzaSyCp3LWWCVszsF_ES1JXA5OsRA27If3CGHU
## Example 
GET https://www.googleapis.com/customsearch/v1?key=AIzaSyCp3LWWCVszsF_ES1JXA5OsRA27If3CGHU&cx=017576662512468239146:omuauf_lfve&q=[text to find]

# Msn Search 
## KEY 
7947afa74a6647eaa3838c2cfc394ebf
## Example
GET https://api.cognitive.microsoft.com/bing/v7.0/news/search?q=test
Header
Ocp-Apim-Subscription-Key 7947afa74a6647eaa3838c2cfc394ebf