﻿using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IActivityLocationTypeRepository
    {
        public IEnumerable<IActivityLocation> ActivityLocationTypes()
        {
            var q = _db.ActivityLocations
                       .AsNoTracking()
                       .Where(i => !i.IsDeleted)
                       .OrderBy(i => i.SortOrder)
                       .Select(i => i);

            return (q);
        }
    }
}

