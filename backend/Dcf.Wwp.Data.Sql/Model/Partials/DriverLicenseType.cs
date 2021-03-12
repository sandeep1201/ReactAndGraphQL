using Dcf.Wwp.Model.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class DriverLicenseType : BaseCommonModel, IDriverLicenseType
    {
        ICollection<IDriverLicense> IDriverLicenseType.DriverLicenses
        {
            get { return DriverLicenses.Cast<IDriverLicense>().ToList(); }
            set { DriverLicenses = value.Cast<DriverLicense>().ToList(); }
        }
    }
}
