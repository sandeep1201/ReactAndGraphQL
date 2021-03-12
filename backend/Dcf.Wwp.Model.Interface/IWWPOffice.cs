using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWWPOffice : ICommonDelModel
    {
        Nullable<int>             OfficeNumber              { get; set; }
        string                    OfficeName                { get; set; }
        Nullable<int>             MFWPOfficeNumber          { get; set; }
        Nullable<int>             MFEligibilityOfficeNumber { get; set; }
        Nullable<int>             CountyandTribeId          { get; set; }
        Nullable<int>             ContractAreaId            { get; set; }
        Nullable<int>             MFLocationNumber          { get; set; }
        Nullable<System.DateTime> ActiviatedDate            { get; set; }
        Nullable<System.DateTime> InactivatedDate           { get; set; }
        IContractArea             ContractArea              { get; set; }
        ICountyAndTribe           CountyAndTribe            { get; set; }
    }
}
