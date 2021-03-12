using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class JRInterviewInfo : BaseEntity
    {
        #region Properties

        public int      JobReadinessId              { get; set; }
        public string   LastInterviewDetails        { get; set; }
        public bool?    CanLookAtSocialMedia        { get; set; }
        public string   CanLookAtSocialMediaDetails { get; set; }
        public bool?    HaveOutfit                  { get; set; }
        public string   HaveOutfitDetails           { get; set; }
        public bool     IsDeleted                   { get; set; }
        public string   ModifiedBy                  { get; set; }
        public DateTime ModifiedDate                { get; set; }

        #endregion

        #region Navigation Properties

        public virtual JobReadiness JobReadiness { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
