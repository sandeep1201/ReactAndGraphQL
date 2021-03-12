namespace Dcf.Wwp.Api.Library.Contracts
{
    public class JRApplicationInfoContract
    {
        public JRApplicationInfoContract()
        {
        }

        public int    Id                               { get; set; }
        public bool?  CanSubmitOnline                  { get; set; }
        public string CanSubmitOnlineDetails           { get; set; }
        public bool?  HaveCurrentResume                { get; set; }
        public string HaveCurrentResumeDetails         { get; set; }
        public bool?  HaveProfessionalReference        { get; set; }
        public string HaveProfessionalReferenceDetails { get; set; }
        public int?   NeedDocumentLookupId             { get; set; }
        public string NeedDocumentLookupName           { get; set; }
        public string NeedDocumentDetail               { get; set; }
    }
}
