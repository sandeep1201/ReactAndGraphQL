using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class WorkerTaskCategory : BaseEntity
    {
        #region Properties

        public string    Code            { get; set; }
        public string    Name            { get; set; }
        public int       SortOrder       { get; set; }
        public bool      IsSystemUseOnly { get; set; }
        public DateTime  EffectiveDate   { get; set; }
        public DateTime? EndDate         { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime  ModifiedDate    { get; set; }
        public string    Description     { get; set; }
        public bool      IsWWLF          { get; set; }
        public bool      IsCF            { get; set; }
        public bool      IsTJTMJ         { get; set; }
        public bool      IsFCDP          { get; set; }
        public bool      IsEA            { get; set; }

        #endregion

        #region Navigation Properties

        //public virtual WorkerTaskList WorkerTaskList { get; set; }

        #endregion
    }
}
