using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IFCDPRfaDetail : ICommonDelModel
    {
        int?      RequestForAssistanceId  { get; set; }
        bool      IsVoluntary             { get; set; }
        int?      CourtOrderedCountyId    { get; set; }
        DateTime? CourtOrderEffectiveDate { get; set; }
        decimal?  KIDSPinNumber           { get; set; }
        string    ReferralSource          { get; set; }

        ICountyAndTribe       CountyAndTribe       { get; set; }
        IRequestForAssistance RequestForAssistance { get; set; }
    }
}
