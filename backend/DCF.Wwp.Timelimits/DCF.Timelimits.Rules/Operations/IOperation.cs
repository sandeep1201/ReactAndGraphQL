using System;
using System.Linq.Expressions;

namespace DCF.Timelimits.Rules.Actions
{
    public interface IOperation<T>
    {
        Expression<Action<T>> ToExpression();
    }
}