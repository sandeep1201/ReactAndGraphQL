using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ParticipantPlacement : BaseEntity
    {
        #region Properties

        public int       ParticipantId      { get; set; }
        public decimal   CaseNumber         { get; set; }
        public int?      PlacementTypeId    { get; set; }
        public DateTime? PlacementStartDate { get; set; }
        public DateTime? PlacementEndDate   { get; set; }
        public DateTime  CreatedDate        { get; set; }
        public bool      IsDeleted          { get; set; }
        public string    ModifiedBy         { get; set; }
        public DateTime  ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant   Participant   { get; set; }
        public virtual PlacementType PlacementType { get; set; }

        #endregion

        #region Clone

        public ParticipantPlacement Clone()
        {
            return new ParticipantPlacement
                   {
                       Id                 = Id,
                       ParticipantId      = ParticipantId,
                       CaseNumber         = CaseNumber,
                       PlacementTypeId    = PlacementTypeId,
                       PlacementStartDate = PlacementStartDate,
                       PlacementEndDate   = PlacementEndDate,
                       CreatedDate        = CreatedDate,
                       IsDeleted          = IsDeleted,
                       ModifiedBy         = ModifiedBy,
                       ModifiedDate       = ModifiedDate
                   };
        }

        #endregion
    }
}
