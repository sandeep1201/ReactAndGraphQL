using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public ICountry CountryByName(string countryName)
        {
            return _db.Countries.FirstOrDefault(x => x.Name.ToLower() == countryName.ToLower());
        }


        /// <summary>
        /// Creates a new "Non Standard" country (i.e. one created by Google API)
        /// </summary>
        /// <param name="parentobject">the State that this Country belongs in</param>
        /// <param name="user"></param>
        /// <returns>new ICountry object</returns>
        public ICountry NewCountry(IState parentobject, string user)
        {
            var c = new Country
                    {
                        IsDeleted     = false,
                        IsNonStandard = true,
                        ModifiedBy    = user,
                        ModifiedDate  = DateTime.Now
                    };

            _db.Countries.Add(c);
            parentobject.Country = c;

            return c;
        }

        /// <summary>
        /// Creates a new "Non Standard" country (i.e. one created by Google API)
        /// </summary>
        /// <param name="parentobject">the City that this Country belongs in</param>
        /// <param name="user"></param>
        /// <returns>new ICountry object</returns>
        public ICountry NewCountry(ICity parentobject, string user)
        {
            var c = new Country()
                    {
                        IsDeleted     = false,
                        IsNonStandard = true,
                        ModifiedBy    = user,
                        ModifiedDate  = DateTime.Now
                    };

            _db.Countries.Add(c);
            parentobject.Country = c;

            return c;
        }

        public IEnumerable<ICountry> Countries()
        {
            return _db.Countries.Where(x => !x.IsNonStandard);
        }
    }
}
