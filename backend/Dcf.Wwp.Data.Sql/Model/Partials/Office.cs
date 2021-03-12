using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Office : BaseEntity, IOffice
    {
        IContractArea IOffice.ContractArea
        {
            get { return ContractArea; }

            set { ContractArea = (ContractArea) value; }
        }

        ICountyAndTribe IOffice.CountyAndTribe
        {
            get { return CountyAndTribe; }

            set { CountyAndTribe = (CountyAndTribe) value; }
        }

        /// <summary>
        /// Returns true if the office is located in milwaukee.
        /// </summary>
        public bool IsLocatedInMilwaukee => CountyandTribeId == Wwp.Model.Interface.Constants.County.Milwaukee;
    }
}
