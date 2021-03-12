using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IAgeCategory
    {
        Int32 Id { get; set; }
        String AgeRange { get; set; }
        String DescriptionText { get; set; }
        ICollection<IChildYouthSectionChild> ChildYouthSectionChilds { get; set; }
    }
}
