﻿using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IMilitaryDischargeTypeRepository
    {
        IMilitaryDischargeType DischargeTypeById(int id);
        IEnumerable<IMilitaryDischargeType> DischargeTypes();
        IEnumerable<IMilitaryDischargeType> AllDischargeTypes();
    }
}