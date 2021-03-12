using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IReferredParticipant
    {
        int Id { get; set; }
        string WamsId { get; set; }
        decimal? PinNumber { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        char MiddleInitialName { get; set; }
        char SuffixName { get; set; }
        short? CountyNumber { get; set; }
        short? OfficeNumber { get; set; }
        string ReferralStatus { get; set; }
        DateTime? WPreferralDate { get; set; }
        string ProgramCode { get; set; }
    }
}
