using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class JRApplicationInfo : BaseEntity
    {
        #region Properties

        public int      JobReadinessId                   { get; set; }
        public bool?    CanSubmitOnline                  { get; set; }
        public string   CanSubmitOnlineDetails           { get; set; }
        public bool?    HaveCurrentResume                { get; set; }
        public string   HaveCurrentResumeDetails         { get; set; }
        public bool?    HaveProfessionalReference        { get; set; }
        public string   HaveProfessionalReferenceDetails { get; set; }
        public int?     NeedDocumentLookupId             { get; set; }
        public string   NeedDocumentDetail               { get; set; }
        public bool     IsDeleted                        { get; set; }
        public string   ModifiedBy                       { get; set; }
        public DateTime ModifiedDate                     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual JobReadiness       JobReadiness       { get; set; }
        public virtual YesNoUnknownLookup NeedDocumentLookup { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
