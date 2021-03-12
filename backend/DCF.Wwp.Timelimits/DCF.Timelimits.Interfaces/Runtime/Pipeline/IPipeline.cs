using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DCF.Core.Runtime.Pipeline
{
    public interface IPipeline<T> where T : class
    {
        IReadOnlyCollection<INode<T>> Nodes { get; }

        IPipeline<T> Add(Type nodeType);
        IPipeline<T> Add(INode<T> node);
        IPipeline<T> Add<TNode>() where TNode : INode<T>;
        Task Execute(T subject);
    }
}