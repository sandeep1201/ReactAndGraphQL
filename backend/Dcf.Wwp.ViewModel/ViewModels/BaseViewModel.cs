using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Logging;

namespace Dcf.Wwp.Api.Library
{
    public class BaseViewModel
    {
        protected IRepository Repo     { get; }
        protected IAuthUser   AuthUser { get; }
        protected ILog        Logger   { get; }

        #region Properties

        // scott v. - this is only used by Participant(s)ViewModels only.
        // It's initialized in those derived types for now. If other VMs need it too,
        // we'll refactor the init here, but for now I want to avoid the overhead for
        // the majority of instances that DO NOT use it.
        protected IReadOnlyCollection<ICountyAndTribe> _countyAndTribes;

        // scott v. - think of this as a C++ macro
        protected Func<short?, IReadOnlyCollection<ICountyAndTribe>, string> _grabCountyName = (countyNumber, countyList) =>
                                                                                               {
                                                                                                   var c = countyList.FirstOrDefault(i => i.CountyNumber == countyNumber);
                                                                                                   return (c != null ? c.CountyName.Trim().ToTitleCase() : string.Empty);
                                                                                               };

        #endregion

        protected BaseViewModel(IRepository repository, IAuthUser authUser)
        {
            Repo     = repository;
            AuthUser = authUser;
            Logger   = LogProvider.GetLogger(GetType());
        }

        protected BaseViewModel(IRepository repository, IAuthUser authUser, IReadOnlyCollection<ICountyAndTribe> countyAndTribes) : this(repository, authUser)
        {
            _countyAndTribes = countyAndTribes;
        }

        private   List<IOrganization> _agencyList;
        protected List<IOrganization> OrgList => _agencyList ?? (_agencyList = Repo.GetOrganizations().ToList());

        private   List<IOffice> _officeList;        
        protected List<IOffice> OfficeList => _officeList ?? (_officeList = Repo.GetOffices());

        protected void MapBaseContractToBaseModel(BaseModelContract contract, ICommonDelCreatedModel model)
        {
            //model.Id = contract.Id; cannot change this    //TODO: use AutoMapper...
            model.ModifiedDate = DateTime.Now;
            model.ModifiedBy   = AuthUser?.Username;
            model.IsDeleted    = contract.IsDeleted;
            model.RowVersion   = contract.RowVersion ?? model.RowVersion;
            model.CreatedDate  = model.CreatedDate   ?? DateTime.Now;
        }
    }
}
