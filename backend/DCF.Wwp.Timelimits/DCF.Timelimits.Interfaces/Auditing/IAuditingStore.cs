using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core.Auditing
{
    public interface IAuditingStore
    {
        /// <summary>
        /// Should save audits to a persistent store.
        /// </summary>
        /// <param name="auditInfo">Audit informations</param>
        Task SaveAsync(IAuditInfo auditInfo);
    }
}
