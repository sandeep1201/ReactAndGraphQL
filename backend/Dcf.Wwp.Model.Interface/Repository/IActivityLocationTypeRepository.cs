﻿using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IActivityLocationTypeRepository
    {
        IEnumerable<IActivityLocation> ActivityLocationTypes();
    }
}