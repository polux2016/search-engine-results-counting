namespace SearchEngineResultsCounting.Contracts
{
    public interface IArgumentsValidator
    {
        string Text { get; }

        bool Validate(string[] args);
    }
}