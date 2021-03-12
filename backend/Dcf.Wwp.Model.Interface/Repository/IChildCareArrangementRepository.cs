using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IChildCareArrangementRepository
    {
        IChildCareArrangement ChildCareArrangementById(int? id);

        IEnumerable<IChildCareArrangement> ChildCareArrangements();
        IEnumerable<IChildCareArrangement> AllChildCareArrangements();
    }
}
