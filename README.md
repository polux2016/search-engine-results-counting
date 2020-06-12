# search-engine-results-counting
Search engine results counting

# Steps to init solution and project
dotnet new sln --name SearchEngineResultsCounting
dotnet new console --name SearchEngineResultsCounting
dotnet sln add SearchEngineResultsCounting/SearchEngineResultsCounting.csproj

# Packages 
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging
dotnet add package Microsoft.Extensions.Logging.Console
dotnet add package Microsoft.Extensions.Http

# Google Search result
## KEY 
AIzaSyCp3LWWCVszsF_ES1JXA5OsRA27If3CGHU
## Example 
GET https://www.googleapis.com/customsearch/v1?key=AIzaSyCp3LWWCVszsF_ES1JXA5OsRA27If3CGHU&cx=017576662512468239146:omuauf_lfve&q=[text to find]
