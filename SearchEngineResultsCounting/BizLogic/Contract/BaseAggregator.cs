using System;
using System.Collections.Generic;
using System.Text;

namespace SearchEngineResultsCounting.BizLogic.Contract
{
    public abstract class BaseAggregator : IAggregator
    {
        public abstract void Append(List<EngineResult> textResults, StringBuilder summaryResult);

        protected bool Validate(List<EngineResult> textResults, StringBuilder summaryResult)
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