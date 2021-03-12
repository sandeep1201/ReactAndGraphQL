using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dcf.Wwp.Api.ActionFilters
{
    public class CustomAuthorizeFilter : AuthorizeFilter
    {
        public CustomAuthorizeFilter(IEnumerable<IAuthorizeData> authorizeData) : base(authorizeData)
        {
        }

        public CustomAuthorizeFilter(string policy) : base(policy)
        {
        }

        public CustomAuthorizeFilter(AuthorizationPolicy policy) : base(policy)
        {
        }

        public CustomAuthorizeFilter(IAuthorizationPolicyProvider policyProvider, IEnumerable<IAuthorizeData> authorizeData) : base(policyProvider, authorizeData)
        {
        }

        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext?.Request?.Method == HttpMethods.Options)
            {
                return;
            }
            else
            {
                await base.OnAuthorizationAsync(context);
            }
            System.Diagnostics.Debug.WriteLine($"Authorization filter async completed.");
        }
    }
}
