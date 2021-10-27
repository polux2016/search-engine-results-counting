using System.Collections.Generic;
using System.Text;

namespace SearchEngineResultsCounting.Services.Contract
{
    public interface IAggregator
    {
        void Append(List<EngineResult> textResults, StringBuilder summaryResult);
    }
}