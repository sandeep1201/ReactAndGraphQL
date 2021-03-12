using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IMilitaryBranchRepository
    {
        IMilitaryBranch MilitaryBranchById(int? id);

        IEnumerable<IMilitaryBranch> MilitaryBranches();
        IEnumerable<IMilitaryBranch> AllMilitaryBranches();
    }
}
