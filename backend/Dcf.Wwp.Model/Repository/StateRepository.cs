using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IStateRepository
    {
        public IEnumerable<IState> States()
        {
            return _db.States.Where(x => !x.IsDeleted).OrderBy(x => x.Code);
        }

        public IEnumerable<IState> USStates()
        {
            return _db.States
                      .Where(i => !i.IsDeleted && i.Country.Name == "United States" && i.Name != "Other")
                      .OrderBy(i => i.Code);
        }

        public IEnumerable<IState> AllStates()
        {
            return _db.States.OrderBy(x => x.Code);
        }

        public IEnumerable<IState> DriversLicenseStates()
        {
            return _db.DriversLicenseStates.Where(x => !x.IsDeleted && !x.State.IsDeleted).OrderBy(x => x.SortOrder).Select(x => x.State);
        }

        public IState StateByCode(string stateCode)
        {
            return _db.States.FirstOrDefault(x => x.Code.ToLower() == stateCode.ToLower());
        }

        public IState StateByCodeAndCountryId(string stateCode, int? countryId)
        {
            return _db.States.FirstOrDefault(x => x.Code.ToLower() == stateCode.ToLower() & x.CountryId == countryId);
        }

        public IState StateById(int? stateId)
        {
            return _db.States.SingleOrDefault(x => x.Id == stateId);
        }

        public IState NewState(ICity city, string user)
        {
            var newState = new State { ModifiedBy = user, ModifiedDate = DateTime.Now, IsNonStandard = true };

            _db.States.Add(newState);

            city.State = newState;

            return (newState);
        }

        public IState NewState(IInvolvedWorkProgram parentObject)
        {
            var st = new State();
            //city.State = st;
            _db.States.Add(st);
            return st;
        }
    }
}
