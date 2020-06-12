namespace SearchEngineResultsCounting.Contracts
{
    public interface IArgumentsValidator
    {
        string[] Texts { get; }

        bool Validate(string[] args);
    }
}