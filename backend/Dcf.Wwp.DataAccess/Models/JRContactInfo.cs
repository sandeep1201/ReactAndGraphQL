using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class JRContactInfo : BaseEntity
    {
        #region Properties

        public int      JobReadinessId                      { get; set; }
        public bool?    CanYourPhoneNumberUsed              { get; set; }
        public string   PhoneNumberDetails                  { get; set; }
        public bool?    HaveAccessToVoiceMailOrTextMessages { get; set; }
        public string   VoiceOrTextMessageDetails           { get; set; }
        public bool?    HaveEmailAddress                    { get; set; }
        public string   EmailAddressDetails                 { get; set; }
        public bool?    HaveAccessDailyToEmail              { get; set; }
        public string   AccessEmailDailyDetails             { get; set; }
        public bool     IsDeleted                           { get; set; }
        public string   ModifiedBy                          { get; set; }
        public DateTime ModifiedDate                        { get; set; }

        #endregion

        #region Navigation Properties

        public virtual JobReadiness JobReadiness { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
