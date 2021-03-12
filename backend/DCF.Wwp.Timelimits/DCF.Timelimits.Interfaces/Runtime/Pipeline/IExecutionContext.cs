using System;

namespace DCF.Core.Runtime.Pipeline
{
    public interface IExecutionContext<out T>
    {
        T Subject { get; }

        IExecutionOptions Options { get; }

        IPipelineNodeResult Results { get; }

        void ChangeSubject(Object newSubject);

        void AddResult(IPipelineNodeResult result);

        Boolean Canceled { get; set; }
    }
}