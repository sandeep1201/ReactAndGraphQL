using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IOtherDemographicRepository
    {
        public IOtherDemographic NewOtherDemographic(IParticipant parent)
        {
            var newOtherDemographic = new OtherDemographic() as IOtherDemographic;

            newOtherDemographic.Participant  = parent;
            newOtherDemographic.ModifiedBy   = _authUser.Username;
            newOtherDemographic.ModifiedDate = DateTime.Now;
            newOtherDemographic.CreatedDate  = _authUser.CDODate ?? DateTime.Now;

            _db.OtherDemographics.Add((OtherDemographic) newOtherDemographic);

            return (newOtherDemographic);
        }
    }
}
