using System.Threading.Tasks;

namespace SearchEngineResultsCounting.Services.Contract
{
    public interface IHttpClientWrapper
    {
        void AddValueToHeader(string key, string[] values);

        Task<T> GetJsonAsync<T>(string url) where T : new();
    }
}
