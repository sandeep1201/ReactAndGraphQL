using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;

namespace Dcf.Wwp.Data.Sql.Model
{
    /// <remarks>
    /// This class Trims() all legacy string values coming back from SqlSvr ;)
    /// </remarks>>
    public partial class StringTrimInterceptor : IDbCommandTreeInterceptor
    {
        #region Properties

        #endregion

        #region Methods

        public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
        {
            if (interceptionContext.OriginalResult.DataSpace != DataSpace.SSpace) return;

            var qryCmd = interceptionContext.Result as DbQueryCommandTree;

            if (qryCmd == null) return;

            var newQuery = qryCmd.Query.Accept(new StringTrimmerQueryVisitor());

            interceptionContext.Result = new DbQueryCommandTree(qryCmd.MetadataWorkspace, qryCmd.DataSpace, newQuery);
        }

        private class StringTrimmerQueryVisitor : DefaultExpressionVisitor
        {
            private static readonly string[] _typesToTrim = { "char", "varchar", "nchar", "nvarchar" };

            public override DbExpression Visit(DbNewInstanceExpression expression)
            {
                var arguments = expression.Arguments.Select(a =>
                                                            {
                                                                var propertyArg = a as DbPropertyExpression;
                                                                if (propertyArg != null && _typesToTrim.Contains(propertyArg.Property.TypeUsage.EdmType.Name))
                                                                {
                                                                    return EdmFunctions.Trim(a);
                                                                }

                                                                return a;
                                                            });

                return (expression.ResultType.New(arguments));
            }
        }

        #endregion
    }
}
