using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ICountryRepository
    {
        ICountry CountryByName(String countryName);
        ICountry NewCountry(IState parentobject, String user);

	    ICountry NewCountry(ICity parentobject, String user);

        IEnumerable<ICountry> Countries();

    }
}
