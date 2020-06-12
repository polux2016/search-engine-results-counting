namespace SearchEngineResultsCounting.Contracts
{
    public interface ISearchEngine
    {
        string Name { get; }

        long GetResultsCount(string text);
    }
}