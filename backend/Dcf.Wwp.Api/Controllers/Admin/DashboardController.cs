using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Controllers.Admin
{
    public class DashboardController : BaseController
    {
        public DashboardController(IRepository repository) : base(repository)
        {
        }
    }
}
