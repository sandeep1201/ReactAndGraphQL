using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DCF.Common.Extensions;
using NRules.Fluent.Dsl;

namespace DCF.Timelimits.Rules.Actions
{

    public class ExpressionOperation<T> : Operation<T>
    {
        private readonly Expression<Action<T>> _expression;

        public ExpressionOperation(Action<T> action)
        {
            this._expression = obj => action(obj);
        }

        //public ExpressionOperation(Expression<Action<T>> expression)
        //{

        //}

        public override Expression<Action<T>> ToExpression()
        {
            return this._expression;
        }
    }


    public class DualOperation<T> : CompositeOperation<T>
    {
        private Action<T> _aggregateOperation;
        public DualOperation(IOperation<T> first, IOperation<T> second) : base(first, second)
        {

        }

        public override Expression<Action<T>> ToExpression()
        {
            //return First.ToExpression().And(Second.ToExpression()); // TODO: Try to figure out how to combine these without compiling

            // .Compile() is expensive, so we will used cached version
            this._aggregateOperation = this._aggregateOperation ?? First.ToExpression().Compile() + Second.ToExpression().Compile();
            return (T) => this._aggregateOperation(T);
        }
    }


}
