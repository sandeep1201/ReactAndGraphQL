using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core.Auditing
{
    public interface IAuditingHelper
    {
        Boolean ShouldSaveAudit(MethodInfo methodInfo, Boolean defaultValue = false);

        IAuditInfo CreateAuditInfo(MethodInfo method, Object[] arguments);
        IAuditInfo CreateAuditInfo(MethodInfo method, IDictionary<String, Object> arguments);

        void Save(IAuditInfo auditInfo);
        Task SaveAsync(IAuditInfo auditInfo);
    }
}
