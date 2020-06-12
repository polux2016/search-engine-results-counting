namespace SearchEngineResultsCounting.Contracts
{
    public interface ISearchEngine
    {
        string Name { get; }

        int GetResultsCount(string text);
    }
}