
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IEmployerOfRecordInformationRepository
    {
        public IEmployerOfRecordInformation NewEmployerOfRecordInformation()
        {
            var eor = new EmployerOfRecordInformation() { };

            return (eor);
        }
        public void DeleteEoRInfo(int eorId)
        {
            var currentEoRRow = _db.EmployerOfRecordInformations.FirstOrDefault(i => i.EmploymentInformationId == eorId);

            if (currentEoRRow != null)
            {
                _db.EmployerOfRecordInformations.Remove(currentEoRRow);
            }
        }
    }
}
