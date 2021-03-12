using System;
using System.Collections.Generic;

namespace DCF.Core.Runtime.Pipeline
{
    public interface IPipelineNodeResult
    {
        Object Subject { get; }
        String Id { get; set; }
        String PipeId { get; set; }
        PipelineNodeStatus Status { get; set; }
        Exception Exception { get; }
        IReadOnlyList<IPipelineNodeResult> Children { get; }
        T Get<T>();
        IEnumerable<Exception> GetExceptions(Boolean includeSucessExceptions = false);
        void AddChild(IPipelineNodeResult item);
        void AddChildren(IEnumerable<IPipelineNodeResult> items);
        void AddExceptions(List<Exception> exceptions, Boolean includeSucessExceptions);

    }
}