using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ICountyAndTribe
    {
        int                  Id                      { get; set; }
        short?               CountyNumber            { get; set; }
        string               CountyName              { get; set; }
        bool                 IsCounty                { get; set; }
        string               AgencyName              { get; set; }
        short?               LocationNumber          { get; set; }
        bool                 IsDeleted               { get; set; }
        string               ModifiedBy              { get; set; }
        byte[]               RowVersion              { get; set; }
        int?                 SortOrder               { get; set; }
        ICollection<IOffice> Offices                 { get; set; }
    }
}
