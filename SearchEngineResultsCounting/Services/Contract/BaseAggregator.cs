using System;
using System.Collections.Generic;
using System.Text;

namespace SearchEngineResultsCounting.Services.Contract
{
    public abstract class BaseAggregator : IAggregator
    {
        public abstract void Append(List<EngineResult> textResults, StringBuilder summaryResult);

        protected virtual bool Validate(List<EngineResult> textResults, StringBuilder summaryResult)
        {
            if (textResults is null)
            {
                throw new ArgumentNullException("textResults");
            }

            if (summaryResult is null)
            {
                throw new ArgumentNullException("summaryResult");
            }

            return true;
        }
    }
}