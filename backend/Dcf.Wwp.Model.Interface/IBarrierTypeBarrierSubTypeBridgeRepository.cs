﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IBarrierTypeBarrierSubTypeBridgeRepository
    {
        IBarrierTypeBarrierSubTypeBridge NewBarrierTypeBarrierSubTypeBridge(IBarrierDetail parentObject, String user);
    }
}