using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
   public interface IApplicationStatusTypeRepository
    {
        IEnumerable<IApplicationStatusType> ApplicationStatusTypes();

        IApplicationStatusType ApplicationStatusTypeById(Int32? id);
    }
}
