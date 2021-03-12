using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    /// <inheritdoc />
    public interface IContentPage : ICommonDelCreatedModel
    {
        String Title { get; set; }
        String Description { get; set; }
        Int32 SortOrder { get; set; }
        String Slug { get;set; }
        ICollection<IContentModule> ContentModules { get; set; }
    }
}
