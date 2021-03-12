namespace Dcf.Wwp.Api.Library.Contracts
{
    public class JRInterviewInfoContract
    {
        public JRInterviewInfoContract()
        {
        }

        public int    Id                          { get; set; }
        public string LastInterviewDetails        { get; set; }
        public bool?  CanLookAtSocialMedia        { get; set; }
        public string CanLookAtSocialMediaDetails { get; set; }
        public bool?  HaveOutfit                  { get; set; }
        public string HaveOutfitDetails           { get; set; }
    }
}
