using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Plan : BaseEntity
    {
        #region Properties

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int PlanNumber { get; set; }

        public int      ParticipantId    { get; set; }
        public int      PlanTypeid       { get; set; }
        public int      PlanStatusTypeid { get; set; }
        public int      OrganizationId   { get; set; }
        public DateTime CreatedDate      { get; set; }
        public bool     IsDeleted        { get; set; }
        public string   ModifiedBy       { get; set; }
        public DateTime ModifiedDate     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant              Participant    { get; set; }
        public virtual PlanType                 PlanType       { get; set; }
        public virtual PlanStatusType           PlanStatusType { get; set; }
        public virtual Organization             Organization   { get; set; }
        public virtual ICollection<PlanSection> PlanSections   { get; set; } = new List<PlanSection>();

        #endregion
    }
}
