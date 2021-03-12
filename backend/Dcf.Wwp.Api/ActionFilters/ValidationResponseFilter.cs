using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dcf.Wwp.Api.ActionFilters
{
    public class ValidationResponseFilter : ActionFilterAttribute
    {
        ///
        /// Called when [action executing].
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuted(ActionExecutedContext actionContext)
        {
            if (actionContext.ModelState.IsValid)
            {
                base.OnActionExecuted(actionContext);
            }
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Result = new BadRequestObjectResult(actionContext.ModelState);
            }
        }

    }
}
