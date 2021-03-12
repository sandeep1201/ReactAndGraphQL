using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface INonCustodialReferralChild : ICommonModel, IHasDeleteReason, ICloneable
    {
        string                      FirstName                  { get; set; }
        string                      LastName                   { get; set; }
        int?                        ReferralContactIntervalId  { get; set; }
        string                      ContactIntervalDetails     { get; set; }
        bool?                       HasChildSupportOrder       { get; set; }
        string                      ChildSupportOrderDetails   { get; set; }
        IReferralContactInterval    ReferralContactInterval    { get; set; }
        INonCustodialReferralParent NonCustodialReferralParent { get; set; }
    }
}
