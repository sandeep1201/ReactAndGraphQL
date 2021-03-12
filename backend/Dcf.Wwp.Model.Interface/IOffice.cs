using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IOffice
    {
        #region Properties

        int       Id                        { get; set; }
        short?    OfficeNumber              { get; set; }
        string    OfficeName                { get; set; }
        short?    MFWPOfficeNumber          { get; set; }
        short?    MFEligibilityOfficeNumber { get; set; }
        int?      CountyandTribeId          { get; set; }
        int?      ContractAreaId            { get; set; }
        short?    MFLocationNumber          { get; set; }
        DateTime? ActiviatedDate            { get; set; }
        DateTime? InactivatedDate           { get; set; }
        bool      IsDeleted                 { get; set; }
        string    ModifiedBy                { get; set; }
        DateTime? ModifiedDate              { get; set; }
        byte[]    RowVersion                { get; set; }
        bool      IsLocatedInMilwaukee      { get; }

        #endregion

        #region Nav Props

        IContractArea   ContractArea   { get; set; }
        ICountyAndTribe CountyAndTribe { get; set; }

        #endregion
    }
}
