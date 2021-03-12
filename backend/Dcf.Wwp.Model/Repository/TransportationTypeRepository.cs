using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Model.Repository
{
    partial class Repository : ITransportationTypeRepository
    {
        public IEnumerable<ITransportationType> GetTransportationTypes()
        {
            var q = _db.TransportationTypes.OrderBy(i => i.SortOrder);

            return (q);
        }

        public List<ITransportationType> GetTransportationTypesWhere(Expression<Func<ITransportationType, bool>> clause)
        {
            try
            {
                return _db.TransportationTypes
                          .Where(clause)
                          .AsNoTracking()
                          .ToList();
            }
            catch (NullReferenceException)
            {
                return new List<ITransportationType>();
            }
        }
    }
}
