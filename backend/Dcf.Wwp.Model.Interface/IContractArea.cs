using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IContractArea : ICommonDelModel
    {
        #region Proeprties

        string    ContractAreaName  { get; set; }
        int?      OrganizationId    { get; set; }
        int?      EnrolledProgramId { get; set; }
        DateTime? ActivatedDate     { get; set; }
        DateTime? InActivatedDate   { get; set; }

        #endregion

        #region Nav Props

        IEnrolledProgram                     EnrolledProgram         { get; set; }
        IOrganization                        Organization            { get; set; }
        ICollection<IOffice>                 Offices                 { get; set; }
        ICollection<IAssociatedOrganization> AssociatedOrganizations { get; set; }

        #endregion
    }
}
