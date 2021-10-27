namespace SearchEngineResultsCounting.Contracts.Configurations
{
    public class GoogleEngineConfiguration
    {
        public const string SectionName = "GoogleEngine";
        
        public string ApiKey { get; set; }

        public bool Enabeld { get; set; }
    }
}
