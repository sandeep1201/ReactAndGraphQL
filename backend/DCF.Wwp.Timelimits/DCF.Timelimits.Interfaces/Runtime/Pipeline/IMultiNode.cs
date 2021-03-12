using System.Collections.Generic;

namespace DCF.Core.Runtime.Pipeline
{
    public interface IMultiNode<T> : INode<T> where T : class
    {
        IReadOnlyList<INode<T>> Children { get; }
        void AddChild(INode<T> child);
        void AddChildren(IEnumerable<INode<T>> children);
        void RemoveChild(INode<T> child);
    }
}
