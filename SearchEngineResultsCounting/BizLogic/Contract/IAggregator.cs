using System.Collections.Generic;
using System.Text;

namespace SearchEngineResultsCounting.BizLogic.Contract
{
    public interface IAggregator
    {
        void Append(List<EngineResult> textResults, StringBuilder summaryResult);
    }
}