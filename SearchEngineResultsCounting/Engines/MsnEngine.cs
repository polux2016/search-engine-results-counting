using SearchEngineResultsCounting.Contracts;

namespace SearchEngineResultsCounting.Engines
{
    public class MsnEngine : ISearchEngine
    {
        public string Name { get; } = "MSN";

        public int GetResultsCount(string text)
        {
            //TODO: Implement the MSN search Engine 
            return text.Length;
        }
    }
}