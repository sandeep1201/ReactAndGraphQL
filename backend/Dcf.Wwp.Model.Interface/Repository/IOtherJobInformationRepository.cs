using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IOtherJobInformationRepository
    {
        IOtherJobInformation OtherJobInformationById(Int32? id);
        IOtherJobInformation NewOtherJobInformation(String user);
    }
}
