using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core.Auditing
{
    /// <summary>
    /// List of selector functions to select classes/interfaces to be audited.
    /// </summary>
    public interface IAuditingSelectorList : IList<INamedTypeSelector>
    {
        /// <summary>
        /// Removes a selector by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Boolean RemoveByName(String name);
    }
}
