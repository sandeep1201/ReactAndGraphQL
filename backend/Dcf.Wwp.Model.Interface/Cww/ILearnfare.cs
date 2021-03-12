using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Cww
{
    public interface ILearnfare
    {
        String FirstName { get; set; }
        String LastName { get; set; }
        String Middle { get; set; }
        DateTime? BirthDate { get; set; }
        String LearnfareStatus { get; set;}
    }
}
