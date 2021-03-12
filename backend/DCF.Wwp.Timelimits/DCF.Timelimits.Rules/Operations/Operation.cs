using System;
using System.Linq.Expressions;

namespace DCF.Timelimits.Rules.Actions
{
    public abstract class Operation<T> : IOperation<T>
    {
        public abstract Expression<Action<T>> ToExpression();

        /// <summary>
        /// Implicitly converts a specification to expression.
        /// </summary>
        /// <param name="specification"></param>
        public static implicit operator Expression<Action<T>>(Operation<T> operation)
        {
            return operation.ToExpression();
        }
    }
}