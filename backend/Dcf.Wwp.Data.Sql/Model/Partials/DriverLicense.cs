using Dcf.Wwp.Model.Interface;
using System.ComponentModel.DataAnnotations;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class DriverLicense : BaseCommonModel, IDriverLicense
    {
        IDriverLicenseType IDriverLicense.DriverLicenseType
        {
            get { return DriverLicenseType; }
            set { DriverLicenseType = (DriverLicenseType) value; }
        }


        IState IDriverLicense.State
        {
            get { return State; }
            set { State = (State) value; }
        }
    }
}
