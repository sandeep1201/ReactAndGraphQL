using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IReferralContactInterval : ICommonDelModel
    {
        String Name { get; set; }
        Int32 SortOrder { get; set; }
    }
}