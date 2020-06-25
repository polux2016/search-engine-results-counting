using System.Threading.Tasks;

namespace SearchEngineResultsCounting.Contracts
{
    public interface ISearchEngine
    {
        string Name { get; }

        Task<long> GetResultsCount(string text);
    }
}