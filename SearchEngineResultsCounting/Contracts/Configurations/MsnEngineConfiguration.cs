namespace SearchEngineResultsCounting.Contracts.Configurations
{
    public class MsnEngineConfiguration
    {
        public const string SectionName = "MsnEngine";

        public string AccessKey { get; set; }

        public bool Enabeld { get; set; }
    }
}
