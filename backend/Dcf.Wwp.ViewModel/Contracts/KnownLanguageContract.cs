namespace Dcf.Wwp.Api.Library.Contracts
{
    public class KnownLanguageContract
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public bool? CanRead { get; set; }
        public bool? CanSpeak { get; set; }
        public bool? CanWrite { get; set; }
        public bool? IsPrimary = false;
    }
}
