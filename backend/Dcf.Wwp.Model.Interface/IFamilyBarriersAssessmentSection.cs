﻿using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IFamilyBarriersAssessmentSection : ICommonModel, ICloneable
    {
        Boolean? ReviewCompleted { get; set; }
    }
}