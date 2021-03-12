using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Organization : BaseEntity, IOrganization
    {
        #region Properties

        #endregion

        #region Nav Properties

        ICollection<IContractArea> IOrganization.ContractAreas
        {
            get => ContractAreas.Cast<IContractArea>().ToList();
            set => ContractAreas = value.Cast<ContractArea>().ToList();
        }

        ICollection<IWorker> IOrganization.Workers
        {
            get => Workers.Cast<IWorker>().ToList();
            set => Workers = value.Cast<Worker>().ToList();
        }

        #endregion
    }
}
