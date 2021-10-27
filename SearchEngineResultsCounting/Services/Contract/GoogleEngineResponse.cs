namespace SearchEngineResultsCounting.Services.Contract
{
    public class GoogleEngineResponse
    {
        public SearchInformation SearchInformation { get; set; } = new SearchInformation();
    }

    public class SearchInformation
    {
        public long TotalResults { get; set; }
    }
}
