using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
   public interface ILicenseType:ICloneable,ICommonModel
    {
        Int32? SortOrder { get; set; }
        String Name { get; set; }
        ICollection<IPostSecondaryLicense> PostSecondaryLicenses { get; set; }
    }
}
