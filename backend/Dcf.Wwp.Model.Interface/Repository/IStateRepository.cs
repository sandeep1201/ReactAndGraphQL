using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IStateRepository
    {
        IEnumerable<IState> States();
        IEnumerable<IState> USStates();
        IEnumerable<IState> AllStates();
        IEnumerable<IState> DriversLicenseStates();
        IState              StateByCode(string             stateCode);
        IState              StateByCodeAndCountryId(string stateCode, int? countryId);
        IState              StateById(int?                 stateId);
        IState              NewState(ICity                 city, string user);
    }
}
