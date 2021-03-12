namespace Dcf.Wwp.Api.Library.Contracts
{
    public class JRContactInfoContract
    {
        public JRContactInfoContract()
        {
        }

        public int    Id                                  { get; set; }
        public bool?  CanYourPhoneNumberUsed              { get; set; }
        public string PhoneNumberDetails                  { get; set; }
        public bool?  HaveAccessToVoiceMailOrTextMessages { get; set; }
        public string VoiceOrTextMessageDetails           { get; set; }
        public bool?  HaveEmailAddress                    { get; set; }
        public string EmailAddressDetails                 { get; set; }
        public bool?  HaveAccessDailyToEmail              { get; set; }
        public string AccessEmailDailyDetails             { get; set; }
    }
}
