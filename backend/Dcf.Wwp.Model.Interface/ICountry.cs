using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ICountry : ICommonModel
    {
        string              Name          { get; set; }
        int?                SortOrder     { get; set; }
        bool                IsNonStandard { get; set; }
        bool                IsDeleted     { get; set; }
        ICollection<IState> States        { get; set; }
        ICollection<ICity>  Cities        { get; set; }
    }
}
