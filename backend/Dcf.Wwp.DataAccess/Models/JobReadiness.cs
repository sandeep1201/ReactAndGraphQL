using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class JobReadiness : BaseEntity
    {
        #region Properties

        public int      ParticipantId { get; set; }
        public bool     IsDeleted     { get; set; }
        public DateTime CreatedDate   { get; set; }
        public string   ModifiedBy    { get; set; }
        public DateTime ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                    Participant         { get; set; }
        public virtual ICollection<JRApplicationInfo> JrApplicationInfos  { get; set; } = new List<JRApplicationInfo>();
        public virtual ICollection<JRContactInfo>     JrContactInfos      { get; set; } = new List<JRContactInfo>();
        public virtual ICollection<JRHistoryInfo>     JrHistoryInfos      { get; set; } = new List<JRHistoryInfo>();
        public virtual ICollection<JRInterviewInfo>   JrInterviewInfos    { get; set; } = new List<JRInterviewInfo>();
        public virtual ICollection<JRWorkPreferences> JrWorkPreferenceses { get; set; } = new List<JRWorkPreferences>();

        #endregion

        #region Clone

        #endregion
    }
}
