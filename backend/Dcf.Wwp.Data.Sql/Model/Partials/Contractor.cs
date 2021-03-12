
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Contractor : BaseCommonModel, IContractor
    {
        IEnrolledProgram IContractor.EnrolledProgram
        {
            get { return EnrolledProgram; }
            set { EnrolledProgram = (EnrolledProgram)value; }
        }
        
        IAgency IContractor.Agency
        {
            get { return Agency; }
            set { Agency = (Agency)value; }
        }
    }
}
