namespace Dcf.Wwp.Api.Library.Contracts
{
    public class CommentTypeContract
    {
        public int    CommentTypeId   { get; set; }
        public string CommentTypeName { get; set; }
        public bool   IsSystemUseOnly { get; set; }
    }
}
