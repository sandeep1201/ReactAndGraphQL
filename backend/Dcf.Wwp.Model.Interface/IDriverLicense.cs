using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IDriverLicense
    {
         Int32? StateId { get; set; }
         Int32? DriverLicenseTypeId { get; set; }
         DateTime? ExpiredDate { get; set; }
         String Details { get; set; }

        IDriverLicenseType DriverLicenseType { get; set; }

        IState State { get; set; }
    }
}
