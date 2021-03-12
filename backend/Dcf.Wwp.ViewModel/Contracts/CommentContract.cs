using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class CommentContract
    {
        public int                       Id             { get; set; }
        public string                    CommentText    { get; set; }
        public List<int>                 CommentTypeIds { get; set; }
        public List<CommentTypeContract> CommentTypes   { get; set; }
        public bool                      IsEdited       { get; set; }
        public DateTime                  CreatedDate    { get; set; }
        public string                    ModifiedBy     { get; set; }
        public DateTime                  ModifiedDate   { get; set; }
        public string                    WIUID          { get; set; }
    }
}
