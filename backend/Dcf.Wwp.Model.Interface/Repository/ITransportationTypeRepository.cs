using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ITransportationTypeRepository
    {
        IEnumerable<ITransportationType> GetTransportationTypes();
        List<ITransportationType> GetTransportationTypesWhere(Expression<Func<ITransportationType, bool>> clause);
    }
}
