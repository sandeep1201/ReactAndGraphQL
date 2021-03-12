using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IState : ICommonModel
    {
        string                      Code           { get; set; }
        string                      Name           { get; set; }
        int?                        CountryId      { get; set; }
        bool                        IsNonStandard  { get; set; }
        bool                        IsDeleted      { get; set; }
        ICountry                    Country        { get; set; }
    }
}
