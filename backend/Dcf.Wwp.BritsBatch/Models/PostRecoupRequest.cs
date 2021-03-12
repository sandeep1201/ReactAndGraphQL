using System;
using System.Collections.Generic;

namespace Dcf.Wwp.BritsBatch.Models
{
    public class PostRecoupRequest
    {
        #region Properties

        public string                   AuthenticatedToken { get; set; }
        public List<PostRecoupmentList> PostRecoupmentList { get; set; }

        #endregion

        #region Methods

        #endregion
    }

    public class PostRecoupmentList
    {
        public long       CaseNumber           { get; set; }
        public List<long> LiablePINList        { get; set; }
        public decimal    PostRecoupmentAmount { get; set; }
        public DateTime   BenefitBeginDate     { get; set; }
        public int        AGSequence           { get; set; }
        public bool       IsRecoupmentOverride { get; set; }
        public string     ProgramCode          { get; set; }
        public string     SubProgramCode       { get; set; }
    }
}
