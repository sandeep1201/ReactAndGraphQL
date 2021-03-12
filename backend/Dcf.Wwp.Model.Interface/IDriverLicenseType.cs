using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
     public interface IDriverLicenseType
    {
        String Name { get; set; }
       
        ICollection<IDriverLicense> DriverLicenses { get; set; }

    }
}
