using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IAssociatedOrganization : ICommonModelFinal
    {
        #region Proeprties

        int       ContractAreaId  { get; set; }
        int       OrganizationId  { get; set; }
        DateTime  ActivatedDate   { get; set; }
        DateTime? InactivatedDate { get; set; }

        #endregion

        #region Nav Props

        IContractArea ContractArea { get; set; }
        IOrganization Organization { get; set; }

        #endregion
    }
}
