using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ITimeLimitState : ICommonDelCreatedModel
    {
        String Code { get; set; }
        String Name { get; set; }
        Int32? CountryId { get; set; }
    }
}