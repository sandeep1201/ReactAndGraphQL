using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace DCF.Core.Threading
{
    public static class AsyncHelper
    {
        public static Boolean IsAsyncMethod(MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                );
        }

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncContext.Run(func);
        }

        public static void RunSync(Func<Task> action)
        {
            AsyncContext.Run(action);
        }
    }
}
