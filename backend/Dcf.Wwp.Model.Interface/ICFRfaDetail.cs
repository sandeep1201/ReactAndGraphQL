using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ICFRfaDetail : ICommonDelModel
    {
        int?      RequestForAssistanceId  { get; set; }
        int?      CourtOrderedCountyId    { get; set; }
        DateTime? CourtOrderEffectiveDate { get; set; }

        ICountyAndTribe       CountyAndTribe       { get; set; }
        IRequestForAssistance RequestForAssistance { get; set; }
    }
}
