
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model 
{
    public partial class WPOrganization : IWPOrganization
    {
        IAgency IWPOrganization.Agency
        {
            get { return Agency; }

            set { Agency = (Agency)value; }
        }

        ICountyAndTribe IWPOrganization.CountyAndTribe
        {
            get { return CountyAndTribe; }

            set { CountyAndTribe = (CountyAndTribe)value; }
        }
    }
}
