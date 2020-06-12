using SearchEngineResultsCounting.Contracts;

namespace SearchEngineResultsCounting.Engines
{
    public class GoogleEngine : ISearchEngine
    {
        public string Name { get; } = "Google Engine";

        public int GetResultsCount(string text)
        {
            //TODO: Implement the Google search Engine 
            return text.Length;
        }
    }
}