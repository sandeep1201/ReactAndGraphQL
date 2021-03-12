using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IAgency : ICommonDelModel
    {
        #region Properties

        int    Id               { get; set; }
        short? AgencyNumber     { get; set; }
        string EntsecAgencyCode { get; set; }
        string AgencyName       { get; set; }
        string GeoAreaName      { get; set; }

        #endregion

        #region Navigation Props

        ICollection<IWorker>         Workers         { get; set; }
        ICollection<IWPOrganization> WPOrganizations { get; set; }
        ICollection<IContractor>     Contractors     { get; set; }

        #endregion
    }
}
