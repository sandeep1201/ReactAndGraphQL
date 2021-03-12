using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class DrugScreening : BaseEntity
    {
        #region Properties

        public int      Id                        { get; set; }
        public int      ParticipantId             { get; set; }
        public bool     UsedNonRequiredDrugs      { get; set; }
        public bool     AbusedMoreDrugs           { get; set; }
        public bool     CannotStopAbusingDrugs    { get; set; }
        public bool     HadBlackoutsFromDrugs     { get; set; }
        public bool     FeelGuiltyAboutDrugs      { get; set; }
        public bool     SpouseComplaintOnDrugs    { get; set; }
        public bool     NeglectedFamilyForDrugs   { get; set; }
        public bool     IllegalActivitiesForDrugs { get; set; }
        public bool     SickFromStoppingDrugs     { get; set; }
        public bool     MedicalProblemsFromDrugs  { get; set; }
        public bool     IsDeleted                 { get; set; }
        public string   ModifiedBy                { get; set; }
        public DateTime ModifiedDate              { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                      Participant           { get; set; }
        public virtual ICollection<DrugScreeningStatus> DrugScreeningStatuses { get; set; } = new List<DrugScreeningStatus>();

        #endregion

        #region Clone

        #endregion
    }
}
