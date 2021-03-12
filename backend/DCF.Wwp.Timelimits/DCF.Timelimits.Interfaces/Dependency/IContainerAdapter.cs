using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core.Dependency
{
    public interface IContainerAdapter : IDisposable
    {
        T Resolve<T>();
        T TryResolve<T>();
    }
}
