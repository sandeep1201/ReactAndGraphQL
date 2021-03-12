using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Core.Dependency
{
    public interface IServiceFactory<out T> where T : class
    {
        T Build();
    }
}
