using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class RequestForAssistanceChild
    {
        #region Properties

        public int      RequestForAssistanceId { get; set; }
        public int      ChildId                { get; set; }
        public bool     IsDeleted              { get; set; }
        public string   ModifiedBy             { get; set; }
        public DateTime ModifiedDate           { get; set; }

        #endregion

        #region Navigation Properties

        public virtual RequestForAssistance RequestForAssistance { get; set; }
        public virtual Child                Child                { get; set; }

        #endregion
    }
}
