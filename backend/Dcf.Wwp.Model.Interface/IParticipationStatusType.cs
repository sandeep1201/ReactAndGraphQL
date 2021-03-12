using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IParticipationStatusType : ICommonModelFinal,ICloneable
    {
        string Code { get; set; }
        string Name { get; set; }
        int SortOrder { get; set; }
        DateTime? EffectiveDate { get; set; }
        DateTime? EndDate { get; set; }
        ICollection<IParticipationStatu> ParticipationStatus { get; set; }
    }
}