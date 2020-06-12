namespace SearchEngineResultsCounting.Contracts
{
    interface IResultsCountingFacade
    {
        string FindAndCompareResults(string[] texts);
    }
}