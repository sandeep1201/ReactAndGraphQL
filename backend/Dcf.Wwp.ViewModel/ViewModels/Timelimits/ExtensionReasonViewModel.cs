using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts.Timelimits;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class ExtensionReasonViewModel : BaseViewModel
    {
        public ExtensionReasonViewModel(IRepository repository, IAuthUser authUser) : base(repository, authUser)
        {
        }

        public IEnumerable<ExtensionReasonContract> GetApprovalExtensionReasons()
        {
            return this.Repo.GetExtensionApprovalReasons().Select(ExtensionReasonContract.Create);
        }

        public IEnumerable<ExtensionReasonContract> GetDenialExtensionReasons()
        {
            return this.Repo.GetExtensionDenialReasons().Select(ExtensionReasonContract.Create);
        }
    }
}